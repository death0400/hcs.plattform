using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hcs.Platform.BaseModels;
using Hcs.Platform.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Hcs.Platform.Core.File.DbManager
{
    public class DbFileManager : IFileManager
    {
        private readonly IFileStorage storage;
        private readonly DbSet<PlatformFile> set;
        private readonly DbContext context;
        private readonly IMemoryCache cache;

        public DbFileManager(IFileStorage storage, DbContext context, IMemoryCache cache)
        {
            this.storage = storage;
            this.set = context.Set<PlatformFile>();
            this.context = context;
            this.cache = cache;
        }
        public async Task<string> Create(Stream stream, string name, string dir, string mimeType)
        {
            var key = await this.storage.Create(stream);
            var file = new PlatformFile
            {
                Name = name,
                MimeType = mimeType,
                Length = stream.Length,
                Key = key,
                Dir = dir,
                Date = DateTime.UtcNow,
            };
            await set.AddAsync(file);
            await context.SaveChangesAsync();
            return key;
        }

        public async Task Delete(string key)
        {
            await this.storage.Delete(key);
            var f = await set.Where(x => x.Key == key).FirstOrDefaultAsync();
            if (f != null)
            {
                set.Remove(f);
                await context.SaveChangesAsync();
                ClearCache(key);
            }
        }

        private void ClearCache(string key)
        {
            cache.Remove($"{nameof(DbFileManager)},{key}");
        }

        public async Task<IPlatformFileInfo> GetFileInfo(string key)
        {
            var info = await cache.GetOrCreateAsync<IPlatformFileInfo>($"{nameof(DbFileManager)},{key}", async (entry) =>
            {
                entry.SlidingExpiration = new TimeSpan(0, 10, 0);
                var e = await set.Where(x => x.Key == key).FirstOrDefaultAsync();
                if (e == null)
                {
                    return null;
                }
                return new PlatformFileInfo
                {
                    Name = e.Name,
                    Dir = e.Dir,
                    ETag = $"{e.Key}{e.Date.Ticks:#0}",
                    MimeType = e.MimeType,
                    Length = e.Length
                };
            });
            return info;
        }

        public async Task UpdateContent(string key, Stream stream)
        {
            var e = await set.Where(x => x.Key == key).FirstOrDefaultAsync();
            if (e != null)
            {
                await storage.Update(key, stream);
                e.Length = stream.Length;
                e.Date = DateTime.UtcNow;
                await context.SaveChangesAsync();
                ClearCache(key);
            }
        }
        public async Task ConfirmFile(string key)
        {
            var e = await set.Where(x => x.Key == key).FirstOrDefaultAsync();
            e.Confirmed = true;
            await context.SaveChangesAsync();
            ClearCache(key);
        }
        public async Task ClearUnConfirmed()
        {
            var before = DateTime.UtcNow.AddDays(-1);
            var toDelete = await set.Where(x => x.Confirmed == false && x.Date <= before).ToArrayAsync();
            set.RemoveRange(toDelete);
            foreach (var k in toDelete.Select(x => x.Key))
            {
                await storage.Delete(k);
                ClearCache(k);
            }
            await context.SaveChangesAsync();
        }
        public async Task Rename(string name, string key)
        {
            var e = await set.Where(x => x.Key == key).FirstOrDefaultAsync();
            if (e != null)
            {
                e.Name = name;
                await context.SaveChangesAsync();
                ClearCache(key);
            }
        }

        public async Task Move(string key, string newDir)
        {
            var e = await set.Where(x => x.Key == key).FirstOrDefaultAsync();
            if (e != null)
            {
                e.Dir = newDir;
                await context.SaveChangesAsync();
                ClearCache(key);
            }
        }

        public Task<Stream> Open(string key)
        {
            return storage.Open(key);
        }
    }
}
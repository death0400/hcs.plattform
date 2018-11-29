using System;
using System.IO;
using System.Threading.Tasks;
using Hcs.Platform.File;
namespace Hcs.Platform.Core.File.LocalDisk
{
    public class LocalDiskFileStorage : IFileStorage
    {
        private readonly string rootPath;

        public LocalDiskFileStorage(string rootPath)
        {
            this.rootPath = rootPath;
        }
        static readonly object createLock = new object();
        public async Task<string> Create(Stream stream)
        {
            string key;
            FileInfo fi;
            lock (createLock)
            {
                do
                {
                    key = Guid.NewGuid().ToString("n");
                    fi = new FileInfo(Path.Combine(rootPath, key));
                } while (fi.Exists);
            }
            using (var fs = fi.Open(FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
            {
                await stream.CopyToAsync(fs);
            }
            return key;

        }

        public async Task Delete(string key)
        {
            var fi = new FileInfo(Path.Combine(rootPath, key));
            if (fi.Exists)
            {
                await Task.Run(() => fi.Delete());
            }
        }

        public Task<Stream> Open(string key)
        {
            var fi = new FileInfo(Path.Combine(rootPath, key));
            if (fi.Exists)
            {
                var stream = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                return Task.FromResult((Stream)stream);
            }
            return null;
        }

        public async Task Update(string key, Stream stream)
        {
            var fi = new FileInfo(Path.Combine(rootPath, key));
            if (fi.Exists)
            {
                await Task.Run(async () =>
                {
                    fi.Delete();
                    using (var fs = fi.Open(FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
                    {
                        await stream.CopyToAsync(fs);
                    }
                });
            }
        }

        public Task<bool> Exists(string key)
        {
            return Task.FromResult(new FileInfo(Path.Combine(rootPath, key)).Exists);
        }
    }
}
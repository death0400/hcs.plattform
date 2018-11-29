using System;
using System.Threading.Tasks;
using Hcs.Platform.Data;
using Microsoft.EntityFrameworkCore;

namespace Hcs.Platform.Data
{
    public interface IDbOperation<T> where T : class
    {
        void Excute(Action<DbSet<T>, DbContext> operation);
        void Excute(Action<DbSet<T>> operation);
        Task ExcuteAsync(Func<DbSet<T>, DbContext, Task> operation);
        Task ExcuteAsync(Func<DbSet<T>, Task> operation);
    }
    class DbOperation<T> : IDbOperation<T> where T : class
    {
        private readonly IScopedDbContext<PlatformDbContext> scoped;

        public DbOperation(IScopedDbContext<PlatformDbContext> scoped)
        {
            this.scoped = scoped;
        }
        public void Excute(Action<DbSet<T>, DbContext> operation)
        {
            operation(scoped.DbContext.Set<T>(), scoped.DbContext);
            scoped.DbContext.SaveChanges();
        }

        public void Excute(Action<DbSet<T>> operation) => Excute((set, ctx) => operation(set));

        public async Task ExcuteAsync(Func<DbSet<T>, DbContext, Task> operation)
        {
            await operation(scoped.DbContext.Set<T>(), scoped.DbContext);
            await scoped.DbContext.SaveChangesAsync();
        }
        public async Task ExcuteAsync(Func<DbSet<T>, Task> operation) => await ExcuteAsync(async (set, ctx) => await operation(set));
    }

}
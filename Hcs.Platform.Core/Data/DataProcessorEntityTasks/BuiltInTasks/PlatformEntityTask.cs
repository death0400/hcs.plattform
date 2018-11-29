using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hcs.Platform.Data;
using Hcs.Platform.User;

namespace Hcs.Platform.Core.Data.DataProcessorEntityTasks.BuiltInTasks
{
    public class PlatformEntityTask<T> : DataProcessorEntityTask<T> where T : class, IPlatformEntity
    {
        private static Lazy<string[]> ignoreProperties;
        private readonly IPlatformUser user;
        static PlatformEntityTask()
        {
            ignoreProperties = new Lazy<string[]>(() =>
            {
                var entityType = typeof(T);
                return entityType.GetProperties().Where(x => x.GetCustomAttributes<IgnoreOnUpdate>().Any()).Select(x => x.Name).ToArray();
            });
        }
        public PlatformEntityTask(IPlatformUser user)
        {
            this.user = user;
        }
        public override async Task PreCreate(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> entity)
        {
            entity.Entity.CreatedTime = DateTime.UtcNow;
            entity.Entity.CreatedBy = user.Id;
            await Task.CompletedTask;
        }
        public override async Task PreUpdate(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> entity)
        {
            entity.Entity.LastUpdatedTime = DateTime.UtcNow;
            entity.Entity.LastUpdatedBy = user.Id;
            entity.Property(x => x.CreatedBy).IsModified = false;
            entity.Property(x => x.CreatedTime).IsModified = false;
            foreach (var ignore in ignoreProperties.Value)
            {
                entity.Property(ignore).IsModified = false;
            }
            await Task.CompletedTask;
        }
    }
}

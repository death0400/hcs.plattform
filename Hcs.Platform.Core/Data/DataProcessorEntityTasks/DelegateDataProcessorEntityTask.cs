using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hcs.Platform.Data
{
    public class DelegateDataProcessorEntityTask<TEntity> : DataProcessorEntityTask<TEntity> where TEntity : class
    {
        public Action<EntityEntry<TEntity>> PreUpdateMethod { get; set; }
        public Action<EntityEntry<TEntity>> PreCreateMethod { get; set; }
        public override async Task PreCreate(EntityEntry<TEntity> entity)
        {
            PreCreateMethod?.Invoke(entity);
            await Task.CompletedTask;
        }
        public override async Task PreUpdate(EntityEntry<TEntity> entity)
        {
            PreUpdateMethod?.Invoke(entity);
            await Task.CompletedTask;
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hcs.Platform.Data
{
    public abstract class DataProcessorEntityTask<TEntity> : IDataProcessorEntityTask<TEntity> where TEntity : class
    {
        public virtual Task PreCreate(EntityEntry<TEntity> entity) => Task.CompletedTask;

        public virtual Task PreUpdate(EntityEntry<TEntity> entity) => Task.CompletedTask;
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hcs.Platform.Data
{
    public interface IDataProcessorEntityTask<TEntity> where TEntity : class
    {
        Task PreUpdate(EntityEntry<TEntity> entity);
        Task PreCreate(EntityEntry<TEntity> entity);
    }
}
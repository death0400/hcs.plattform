using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hcs.Platform.Data
{
    public interface IDataProcessorEntityTaskCollection<TEntity> : IReadOnlyCollection<IDataProcessorEntityTask<TEntity>> where TEntity : class
    {
        Task RunTasks(EntityEntry<TEntity> entityEntry, string taskName);
    }
    public static class IDataProcessorEntityTaskCollection
    {
        public const string PreUpdate = nameof(PreUpdate);
        public const string PreCreate = nameof(PreCreate);
    }
}

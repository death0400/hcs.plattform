using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Hcs.Platform.Data
{
    internal class DataProcessorEntityTaskCollection<TEntity> : IDataProcessorEntityTaskCollection<TEntity> where TEntity : class
    {
        IReadOnlyCollection<IDataProcessorEntityTask<TEntity>> tasks;
        public DataProcessorEntityTaskCollection(IServiceProvider serviceProvider)
        {
            tasks = serviceProvider.GetServices<IDataProcessorEntityTask<TEntity>>().ToArray();
        }
        public int Count => tasks.Count;

        public IEnumerator<IDataProcessorEntityTask<TEntity>> GetEnumerator() => tasks.GetEnumerator();

        public async Task RunTasks(EntityEntry<TEntity> entityEntry, string taskName)
        {
            foreach (var t in tasks)
            {
                switch (taskName)
                {
                    case IDataProcessorEntityTaskCollection.PreCreate:
                        await t.PreCreate(entityEntry);
                        break;
                    case IDataProcessorEntityTaskCollection.PreUpdate:
                        await t.PreUpdate(entityEntry);
                        break;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => tasks.GetEnumerator();
    }
}

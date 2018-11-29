using System;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hcs.Platform.Data;
using Hcs.Platform.User;

namespace Hcs.Platform.Data
{
    public class UpdateData<TEntity, TDbContext> : DbSetFlowProcessor<TEntity, TEntity, TEntity, TDbContext> where TEntity : class where TDbContext : DbContext
    {
        private readonly IDataProcessorEntityTaskCollection<TEntity> tasks;
        public UpdateData(IScopedDbContext<TDbContext> context, IDataProcessorEntityTaskCollection<TEntity> tasks) : base(context)
        {
            this.tasks = tasks;
        }
        public override async Task ProcessAsync(TEntity input, Action<TEntity> done)
        {
            var entity = Context.Entry(input);
            if (entity.State != EntityState.Modified)
            {
                entity.State = EntityState.Modified;
            }
            await tasks.RunTasks(entity, IDataProcessorEntityTaskCollection.PreUpdate);
            if (SaveChanges)
            {
                await Context.SaveChangesAsync();
            }
            done(input);
        }

    }
}
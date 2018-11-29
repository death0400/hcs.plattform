using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Hcs.Platform.Data
{
    public class CreateData<TEntity, TDbContext> : DbSetFlowProcessor<TEntity, TEntity, TEntity, TDbContext> where TEntity : class where TDbContext : DbContext
    {
        private readonly IDataProcessorEntityTaskCollection<TEntity> tasks;

        public CreateData(IScopedDbContext<TDbContext> context, IDataProcessorEntityTaskCollection<TEntity> tasks) : base(context)
        {
            this.tasks = tasks;
        }

        public override async Task ProcessAsync(TEntity input, Action<TEntity> done)
        {
            var entity = Context.Entry(input);
            if (entity.State != EntityState.Added)
            {
                entity.State = EntityState.Added;
            }
            await tasks.RunTasks(entity, IDataProcessorEntityTaskCollection.PreCreate);
            if (SaveChanges)
            {
                await Context.SaveChangesAsync();
            }
            done(input);
        }
    }
}
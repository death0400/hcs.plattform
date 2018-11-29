using Hcs.Platform.Flow;
using Microsoft.EntityFrameworkCore;
namespace Hcs.Platform.Data
{
    public abstract class DbSetFlowProcessor<TInput, TOutput, TEntity, TDbContext> : AsyncFlowProcessor<TInput, TOutput> where TEntity : class where TDbContext : DbContext
    {
        public bool SaveChanges { get; set; } = true;
        protected TDbContext Context { get; }
        protected DbSet<TEntity> Set { get; }

        public DbSetFlowProcessor(IScopedDbContext<TDbContext> context)
        {
            Context = context.DbContext;
            Set = Context.Set<TEntity>();
        }
    }
}
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
namespace Hcs.Platform.Data
{
    public class DeleteData<TEntity, TDbContext> : DbSetFlowProcessor<TEntity, TEntity, TEntity, TDbContext> where TEntity : class where TDbContext : DbContext
    {
        public DeleteData(IScopedDbContext<TDbContext> context) : base(context) { }

        public override async Task ProcessAsync(TEntity entity, Action<TEntity> done)
        {
            if (entity != null)
            {
                var entry = Context.Entry(entity);
                if (entry.State != EntityState.Deleted)
                {
                    entry.State = EntityState.Deleted;
                }
                if (SaveChanges)
                {
                    await Context.SaveChangesAsync();
                }
                done(entity);
            }
            else
            {
                done(null);
            }
        }
    }
}
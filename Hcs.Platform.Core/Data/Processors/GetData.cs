using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
namespace Hcs.Platform.Data
{
    public class GetData<TKey, TEntity, TDbContext> : DbSetFlowProcessor<TKey, TEntity, TEntity, TDbContext> where TEntity : class where TDbContext : DbContext
    {
        public GetData(IScopedDbContext<TDbContext> context) : base(context)
        {
        }

        public override async Task ProcessAsync(TKey input, Action<TEntity> done)
        {
            var model = await Set.FindAsync(input);
            done(model);
        }
    }
}
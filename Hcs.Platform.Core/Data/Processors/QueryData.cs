using System.Linq;
using System.Collections.Generic;
using Hcs.Platform.Flow;
using Microsoft.EntityFrameworkCore;

namespace Hcs.Platform.Data
{
    public class QueryData<TInput, TEntity, TDbContext> : FlowProcessor<TInput, IQueryable<TEntity>> where TEntity : class where TDbContext : DbContext
    {
        private readonly TDbContext context;

        public QueryData(TDbContext context)
        {
            this.context = context;
        }

        public override IQueryable<TEntity> Process(TInput input) => context.Set<TEntity>();
    }

}
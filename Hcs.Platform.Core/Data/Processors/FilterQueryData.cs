using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using Hcs.Platform.Flow;

namespace Hcs.Platform.Data
{
    public class FilterQueryData<TEntity> : FlowProcessor<IQueryable<TEntity>, IQueryable<TEntity>>
    {
        private readonly Expression<Func<TEntity, bool>> filter;

        public FilterQueryData(Expression<Func<TEntity, bool>> filter)
        {
            this.filter = filter;
        }

        public override IQueryable<TEntity> Process(IQueryable<TEntity> input) => input.Where(filter);
    }
}
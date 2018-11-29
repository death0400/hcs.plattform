using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hcs.Platform.Data;
using Hcs.Platform.Flow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform.Data
{
    public static class ApiBuilderExtensions
    {
        public static IDependencyInjectionFlowBuilderContext<T> QueryChildFor<T, TChild>(this IDependencyInjectionFlowBuilderContext<T> builder,
     Expression<Func<T, IEnumerable<TChild>>> getChildsExp)
        where T : class where TChild : class => builder.QueryChildFor<PlatformDbContext, T, TChild>(getChildsExp);
        public static IDependencyInjectionFlowBuilderContext<T> QueryChildFor<TDbContext, T, TChild>(this IDependencyInjectionFlowBuilderContext<T> builder,
        Expression<Func<T, IEnumerable<TChild>>> getChildsExp)
          where TDbContext : DbContext where T : class where TChild : class
        {
            return builder.Pipe(async (IScopedDbContext<TDbContext> scopedContext, T model) =>
            {
                await scopedContext.DbContext.Entry(model).Collection(getChildsExp).LoadAsync();
            });
        }
        public static IDependencyInjectionFlowBuilderContext<T> SaveChildsFor<T, TChild>(this IDependencyInjectionFlowBuilderContext<T> builder, Expression<Func<T, IEnumerable<TChild>>> getChildsExp)
            where T : class where TChild : class => builder.SaveChildsFor<PlatformDbContext, T, TChild>(getChildsExp);
        public static IDependencyInjectionFlowBuilderContext<T> SaveChildsFor<TDbContext, T, TChild>(this IDependencyInjectionFlowBuilderContext<T> builder, Expression<Func<T, IEnumerable<TChild>>> getChildsExp)
        where TDbContext : DbContext where T : class where TChild : class
        {
            return builder.Pipe(async (IScopedDbContext<TDbContext> scopedContext,
              IModelInfo<TDbContext, TChild> mit,
              IModelInfo<TDbContext, T> mi,
              UpdateData<TChild, TDbContext> upd,
              CreateData<TChild, TDbContext> cre,
              DeleteData<TChild, TDbContext> del,
              T model) =>
            {
                var getChilds = getChildsExp.Compile();
                var childs = getChilds(model).ToArray();
                var p = Expression.Parameter(typeof(T), "x");
                var w = Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Property(p, mi.PrimaryKey.Properties[0].PropertyInfo), Expression.Constant(mi.PrimaryKeyAccessor.GetValue(model))), p);
                var old = await scopedContext.DbContext.Set<T>().AsNoTracking().Include(getChildsExp).Where(w).SelectMany(getChildsExp).ToArrayAsync();
                upd.SaveChanges = false;
                cre.SaveChanges = false;
                del.SaveChanges = false;
                var newIds = childs.Select(x => mit.PrimaryKeyAccessor.GetValue(x)).Where(x => x != null).ToArray();
                foreach (var child in old.Where(x => !newIds.Any(y => y.Equals(mit.PrimaryKeyAccessor.GetValue(x)))))
                {
                    await del.Run(child);
                }
                foreach (var child in childs)
                {
                    if (mit.PrimaryKeyAccessor.IsNotDefault(child))
                    {
                        await upd.Run(child);
                    }
                    else
                    {
                        await cre.Run(child);
                    }
                }
                await scopedContext.DbContext.SaveChangesAsync();
            });
        }
    }

}
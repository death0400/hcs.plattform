using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
namespace System.Linq
{
    public static class SelectExcludeExtension
    {
        static ConcurrentDictionary<string, Expression> cache = new ConcurrentDictionary<string, Expression>();
        public static IQueryable<T> SelectExclude<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] excludes)
        {
            var type = typeof(T);
            var properties = excludes.Select(x => x.GetPropertyVistPath().Select(m => m.Name).First());
            var exp = cache.GetOrAdd($"{type.FullName}.{string.Join(",", properties)}", (key) =>
            {
                var parameter = Expression.Parameter(type, "x");
                var ctor = Expression.New(type);
                var construction = Expression.MemberInit(ctor, type.GetProperties().Where(x => !properties.Contains(x.Name)).Select(propertyInfo =>
                  {
                      return Expression.Bind(propertyInfo, Expression.Property(parameter, propertyInfo));
                  }));
                return Expression.Lambda<Func<T, T>>(construction, parameter);
            });
            return queryable.Select((Expression<Func<T, T>>)exp);
        }
    }
}
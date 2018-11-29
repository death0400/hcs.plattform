using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
namespace Hcs.Platform.Core
{
    public static class CacheKeyBuilder
    {
        public static string UserRole(long userId) => $"Role-{userId}";
        public static string UserRoleCode(long userId) => $"RoleCode-{userId}";
        public static string User(long userId) => $"User-{userId}";

        public static string UserOdataPermission(long userId) => $"OdataPermission-{userId}";
        static CacheKeyBuilder()
        {
            builders = new Lazy<Func<long, string>[]>(() =>
            {
                var type = typeof(CacheKeyBuilder);
                var list = new List<Func<long, string>>();
                foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    var ps = m.GetParameters();
                    if (ps.Length == 1 && ps[0].ParameterType == typeof(long) && m.ReturnType == typeof(string))
                    {
                        var parameter = Expression.Parameter(typeof(long));
                        var exp = Expression.Lambda<Func<long, string>>(Expression.Call(m, parameter), parameter);
                        list.Add(exp.Compile());
                    }
                }
                return list.ToArray();
            });
        }
        static Lazy<Func<long, string>[]> builders;
        public static IEnumerable<string> GetAllKeys(long userId)
        {
            return builders.Value.Select(x => x(userId));
        }
    }
}

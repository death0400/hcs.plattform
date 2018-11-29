using System;
using System.Collections.Generic;
using System.Linq;
namespace System
{
    public static class HcsExtensionsType
    {
        //source https://stackoverflow.com/questions/4185521/c-sharp-get-generic-type-name/26429045
        static string GetFriendlyName(Type type, bool fullName)
        {
            if (type.IsGenericType)
            {
                if (type.IsGenericTypeDefinition)
                {
                    var donts = Enumerable.Range(0, type.GetGenericArguments().Length - 1).Select(x => ",").Aggregate((x, y) => x + y);
                    var name = $"{type.Name.Split('`')[0]}<{donts}>";
                    return fullName ? $"{type.Namespace}.{name}" : name;
                }
                else
                {
                    var name = type.Name.Substring(0, type.Name.IndexOf('`'));
                    var types = string.Join(",", type.GetGenericArguments().Select(x => GetFriendlyName(x, fullName)));
                    var typeName = $"{name}<{types}>";
                    return fullName ? $"{type.Namespace}.{typeName}" : typeName;
                }
            }
            else
            {
                return fullName ? type.FullName : type.Name;
            }
        }
        /// <summary>
        /// Get Friendly Type Name For Generic Type
        /// </summary>
        /// <param name="type">target type</param>
        /// <returns>Friendly Type Name</returns>
        public static string GetFriendlyName(this Type type) => GetFriendlyName(type, false);
        /// <summary>
        /// Get Friendly Type Name For Generic Type
        /// </summary>
        /// <param name="type">target type</param>
        /// <returns>Friendly Type Name</returns>
        public static string GetFriendlyFullName(this Type type) => GetFriendlyName(type, true);

        static IEnumerable<Type> GetInterfaces(Type type, HashSet<Type> dist)
        {
            var interfaces = type.GetInterfaces();
            foreach (var interf in interfaces)
            {
                if (!dist.Contains(interf))
                {
                    dist.Add(interf);
                    yield return interf;
                    foreach (var iinterf in GetInterfaces(interf, dist))
                    {
                        yield return iinterf;
                    }
                }
            }
        }
        public static bool IsAssignableTo(this Type source, Type dist) => dist.IsAssignableFrom(source);
        public static IEnumerable<Type> GetInheritanceTypes(this Type type)
        {
            var types = new HashSet<Type>();

            while (type != null)
            {
                if (!types.Contains(type))
                {
                    types.Add(type);
                    yield return type;
                }
                foreach (var iinterf in GetInterfaces(type, types))
                {
                    yield return iinterf;
                }
                type = type.BaseType;
            }
        }
    }
}

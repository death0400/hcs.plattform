using Hcs.Platform.PlatformModule;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hcs.Platform.Core.PlatformModule
{
    internal static class EntityApiContextContainer
    {
        static Dictionary<string, EntityApiContext> contexts = new Dictionary<string, EntityApiContext>();
        static Dictionary<string, string> controllerNameMap = new Dictionary<string, string>();
        public static IEnumerable<EntityApiContext> Contexts => contexts.Values;
        public static IEnumerable<Type> Controllers => contexts.Values.Select(x => x.ControllerType).Distinct();
        public static void Add(EntityApiContext context)
        {
            var key = context.Key;
            contexts.AddOrUpdate(key, context);
            controllerNameMap.AddOrUpdate(context.Key.ToLowerInvariant(), context.ControllerName);
        }
        public static EntityApiContext Get(string key) => contexts.GetIfExists(key);
        public static string GetController(string apiKey, string method) => controllerNameMap.GetIfExists((apiKey + "." + method).ToLowerInvariant());
    }
}
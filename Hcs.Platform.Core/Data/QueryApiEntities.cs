using System.Collections.Concurrent;
using System;
namespace Hcs.Platform.Data
{
    public static class QueryApiEntities
    {
        public static ConcurrentBag<Type> Types = new ConcurrentBag<Type>();
    }
}
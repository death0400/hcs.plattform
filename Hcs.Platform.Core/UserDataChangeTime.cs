using System;
using System.Linq;
using System.Collections.Concurrent;
namespace Hcs.Platform.Core
{
    static class UserDataChangeTime
    {
        static ConcurrentDictionary<long, DateTime> timeMap = new ConcurrentDictionary<long, DateTime>();
        public static void Update(long userId, DateTime time)
        {
            timeMap.AddOrUpdate(userId, time, (k, v) => time);
        }
        public static DateTime Get(long userId)
        {
            return timeMap.TryGetValue(userId, out DateTime d) ? d : DateTime.MinValue;
        }
    }
}
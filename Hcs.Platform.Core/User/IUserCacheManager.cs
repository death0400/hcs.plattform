using System;
using Hcs.Platform.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Hcs.Platform.User
{
    public interface IUserCacheManager
    {
        void Clear(long userId);
    }
    internal class UserCacheManager : IUserCacheManager
    {
        private readonly IMemoryCache cache;

        public UserCacheManager(IMemoryCache cache)
        {
            this.cache = cache;
        }
        public void Clear(long userId)
        {
            UserDataChangeTime.Update(userId, DateTime.UtcNow);
            foreach (var k in CacheKeyBuilder.GetAllKeys(userId))
            {
                cache.Remove(k);
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Hcs.Platform
{
    public static class IDistributedCacheExtesnions
    {
        public static T GetObject<T>(this IDistributedCache cache, string key) where T : class => cache.Get(key)?.DeserializeBinaryToObject<T>();
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache, string key) where T : class => (await cache.GetAsync(key))?.DeserializeBinaryToObject<T>();
        public static void SetObject<T>(this IDistributedCache cache, string key, T value) where T : class => cache.Set(key, value.SerializeObjectToBinary());
        public static void SetObject<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options) where T : class => cache.Set(key, value.SerializeObjectToBinary(), options);

        public static async Task SetObjectAsync<T>(this IDistributedCache cache, string key, T value) where T : class => await cache.SetAsync(key, value.SerializeObjectToBinary());
        public static async Task SetObjectAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options) where T : class => await cache.SetAsync(key, value.SerializeObjectToBinary(), options);
    }
}

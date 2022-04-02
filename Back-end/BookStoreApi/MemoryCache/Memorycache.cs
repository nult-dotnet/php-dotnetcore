using Microsoft.Extensions.Caching.Memory;

namespace BookStoreApi.MemoryCaches
{
    public static class Memorycache
    {
        public static MemoryCacheEntryOptions SetMemoryCache()
        {
            MemoryCacheEntryOptions cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromSeconds(20)
            };
            return cacheExpiryOptions;
        }
        public static void SetMemoryCacheAction(IMemoryCache cache)
        {
            var cacheKey = "action";
            cache.Set(cacheKey, true,SetMemoryCache());
        }
        public static bool CheckMemoryCacheAction(IMemoryCache cache)
        {
            var cacheKeyAction = "action";
            return cache.TryGetValue(cacheKeyAction, out bool action);
        }
        public static void RemoveMemoryCacheAction(IMemoryCache cache)
        {
            var cacheKeyAction = "action";
            cache.Remove(cacheKeyAction);
        }
    }
}
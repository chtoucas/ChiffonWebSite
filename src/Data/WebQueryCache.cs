namespace Chiffon.Data
{
    using System;
    using System.Web;
    using System.Web.Caching;
    using Narvalo;

    public class WebQueryCache : IQueryCache
    {
        const int CacheExpirationInHours_ = 24;

        static object Lock_ = new Object();

        readonly HttpContextBase _context;

        public WebQueryCache(HttpContextBase context)
        {
            Requires.NotNull(context, "context");

            _context = context;
        }

        public T GetOrInsert<T>(string cacheKey, Func<T> query) where T : class
        {
            T result = null;

            var cache = _context.Cache;
            var cachedValue = cache[cacheKey];

            if (cachedValue == null) {
                result = query.Invoke();

                lock (Lock_) {
                    if (cache[cacheKey] == null) {
                        cache.Add(cacheKey, result, null,
                            DateTime.Now.AddHours(CacheExpirationInHours_),
                            Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    }
                }
            }
            else {
                result = cachedValue as T;
            }

            return result;
        }
    }
}
namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;
    using System.Web.Caching;
    using Narvalo;

    public class ChiffonCacher : IChiffonCacher
    {
        const int CacheExpirationInHours_ = 1;

        static object Lock_ = new Object();

        readonly HttpContextBase _context;

        // NB: Constructeur pour Autofac.
        // TODO: Je n'aime pas l'utilisation de HttpContext.Current, il faudra voir 
        // comment injecter HttpContextBase via Autofac sans avoir à installer Autofac.Integration.Web.
        public ChiffonCacher() : this(new HttpContextWrapper(HttpContext.Current)) { }

        public ChiffonCacher(HttpContextBase context)
        {
            Requires.NotNull(context, "context");

            _context = context;
        }

        protected Cache Cache { get { return _context.Cache; } }

        public T GetOrInsert<T>(string cacheKey, Func<T> query) where T : class
        {
            T result;

            var cachedValue = Cache[cacheKey];

            if (cachedValue == null) {
                result = query.Invoke();

                lock (Lock_) {
                    if (Cache[cacheKey] == null) {
                        Cache.Add(cacheKey, result, null,
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
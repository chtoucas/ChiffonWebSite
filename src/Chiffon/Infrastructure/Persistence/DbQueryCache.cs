namespace Chiffon.Infrastructure.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.Caching;

    using Chiffon.Entities;
    using Narvalo;

    // REVIEW: Utiliser plutôt des références "faibles" ?
    public class DbQueryCache : IDbQueryCache
    {
        private const int CACHE_EXPIRATION_IN_HOURS = 24;

        private static readonly object s_Lock = new Object();

        private readonly HttpContextBase _context;

        // NB: Constructeur pour Autofac.
        // TODO: Je n'aime pas l'utilisation de HttpContext.Current, il faudra voir 
        // comment injecter HttpContextBase via Autofac sans avoir à installer Autofac.Integration.Web.
        public DbQueryCache() : this(new HttpContextWrapper(HttpContext.Current)) { }

        public DbQueryCache(HttpContextBase context)
        {
            Require.NotNull(context, "context");

            _context = context;
        }

        protected Cache Cache { get { return _context.Cache; } }

        public IEnumerable<Category> GetOrInsertCategories(DesignerKey designerKey, Func<DesignerKey, IEnumerable<Category>> query)
        {
            var cacheKey = "Chiffon:Category:" + designerKey.ToString();
            return GetOrInsert_(cacheKey, () => query(designerKey));
        }

        public IEnumerable<Designer> GetOrInsertDesigners(CultureInfo culture, Func<CultureInfo, IEnumerable<Designer>> query)
        {
            Require.NotNull(culture, "culture");

            var cacheKey = "Chiffon:Designer:" + culture.ToString();
            return GetOrInsert_(cacheKey, () => query(culture));
        }

        public IEnumerable<Pattern> GetOrInsertPatterns(DesignerKey designerKey, Func<DesignerKey, IEnumerable<Pattern>> query)
        {
            var cacheKey = "Chiffon:Pattern:" + designerKey.ToString();
            return GetOrInsert_(cacheKey, () => query(designerKey));
        }

        public IEnumerable<Pattern> GetOrInsertShowcasedPatterns(Func<IEnumerable<Pattern>> query)
        {
            var cacheKey = "Chiffon:Home";
            return GetOrInsert_(cacheKey, () => query());
        }

        private T GetOrInsert_<T>(string cacheKey, Func<T> query) where T : class
        {
            var cachedValue = Cache[cacheKey];

            if (cachedValue != null)
            {
                return cachedValue as T;
            }

            T result = query.Invoke();

            lock (s_Lock)
            {
                if (Cache[cacheKey] == null)
                {
                    Cache.Add(
                        cacheKey, 
                        result, 
                        null,
                        DateTime.Now.AddHours(CACHE_EXPIRATION_IN_HOURS),
                        Cache.NoSlidingExpiration,
                        CacheItemPriority.High, 
                        null);
                }
            }

            return result;
        }
    }
}
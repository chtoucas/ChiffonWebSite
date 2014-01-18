namespace Chiffon.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.Caching;
    using Chiffon.Entities;
    using Narvalo;

    public class DbQueryCache : IDbQueryCache
    {
        const int CacheExpirationInHours_ = 24;

        static object Lock_ = new Object();

        readonly HttpContextBase _context;

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

        #region IDbQueryCache

        public IEnumerable<Category> GetOrInsertCategories(DesignerKey designerKey, Func<DesignerKey, IEnumerable<Category>> query)
        {
            var format = "Chiffon:Category:{0}";
            var cacheKey = String.Format(CultureInfo.InvariantCulture, format, designerKey.ToString());
            return GetOrInsert_(cacheKey, () => query(designerKey));
        }

        public IEnumerable<Designer> GetOrInsertDesigners(CultureInfo culture, Func<CultureInfo, IEnumerable<Designer>> query)
        {
            var format = "Chiffon:Designer:{0}";
            var cacheKey = String.Format(CultureInfo.InvariantCulture, format, culture);
            return GetOrInsert_(cacheKey, () => query(culture));
        }

        public IEnumerable<Pattern> GetOrInsertPatterns(DesignerKey designerKey, Func<DesignerKey, IEnumerable<Pattern>> query)
        {
            var format = "Chiffon:Pattern:{0}";
            var cacheKey = String.Format(CultureInfo.InvariantCulture, format, designerKey.ToString());
            return GetOrInsert_(cacheKey, () => query(designerKey));
        }

        public IEnumerable<Pattern> GetOrInsertShowcasedPatterns(Func<IEnumerable<Pattern>> query)
        {
            var cacheKey = "Chiffon:Home";
            return GetOrInsert_(cacheKey, () => query());
        }

        #endregion

        T GetOrInsert_<T>(string cacheKey, Func<T> query) where T : class
        {
            var cachedValue = Cache[cacheKey];

            if (cachedValue != null) {
                return cachedValue as T;
            }

            T result = query.Invoke();

            lock (Lock_) {
                if (Cache[cacheKey] == null) {
                    Cache.Add(cacheKey, result, null,
                        DateTime.Now.AddHours(CacheExpirationInHours_),
                        Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }

            return result;
        }
    }
}
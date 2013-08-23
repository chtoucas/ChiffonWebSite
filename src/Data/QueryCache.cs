namespace Chiffon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.Caching;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;

    public class QueryCache : IQueryCache
    {
        const int CacheExpirationInHours_ = 24;

        static object Lock_ = new Object();

        readonly HttpContextBase _context;

        // NB: Constructeur pour Autofac.
        // TODO: Je n'aime pas l'utilisation de HttpContext.Current, il faudra voir 
        // comment injecter HttpContextBase via Autofac sans avoir à installer Autofac.Integration.Web.
        public QueryCache() : this(new HttpContextWrapper(HttpContext.Current)) { }

        public QueryCache(HttpContextBase context)
        {
            Requires.NotNull(context, "context");

            _context = context;
        }

        protected Cache Cache { get { return _context.Cache; } }

        #region IChiffonCache

        public IEnumerable<Pattern> GetOrInsertShowcasedPatterns(Func<IEnumerable<Pattern>> query)
        {
            var cacheKey = "Chiffon:Home";
            return GetOrInsert(cacheKey, () => query());
        }

        public IEnumerable<Category> GetOrInsertCategories(DesignerKey designerKey, Func<DesignerKey, IEnumerable<Category>> query)
        {
            var format = "Chiffon:Category:{0}";
            var cacheKey = String.Format(CultureInfo.InvariantCulture, format, designerKey.ToString());
            return GetOrInsert(cacheKey, () => query(designerKey));
        }

        public IEnumerable<Designer> GetOrInsertDesigners(ChiffonCulture culture, Func<ChiffonCulture, IEnumerable<Designer>> query)
        {
            var format = "Chiffon:Designer:{0}";
            var cacheKey = String.Format(CultureInfo.InvariantCulture, format, culture);
            return GetOrInsert(cacheKey, () => query(culture));
        }

        public IEnumerable<Pattern> GetOrInsertPatterns(DesignerKey designerKey, Func<DesignerKey, IEnumerable<Pattern>> query)
        {
            var format = "Chiffon:Pattern:{0}";
            var cacheKey = String.Format(CultureInfo.InvariantCulture, format, designerKey.ToString());
            return GetOrInsert(cacheKey, () => query(designerKey));
        }

        #endregion

        protected T GetOrInsert<T>(string cacheKey, Func<T> query) where T : class
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
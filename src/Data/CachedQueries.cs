namespace Chiffon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo;

    public class CachedQueries : IQueries
    {
        const string DesignerViewModelCacheKey_ = "Chiffon:Designer:{0}:{1}";
        const string HomeCacheKey_ = "Chiffon:Home";
        const string PatternCacheKey_ = "Chiffon:Pattern:{0}";

        readonly IQueries _inner;
        readonly IQueryCache _queryCache;

        public CachedQueries(IQueries inner, IQueryCache queryCache)
        {
            Requires.NotNull(inner, "inner");
            Requires.NotNull(queryCache, "queryCache");

            _inner = inner;
            _queryCache = queryCache;
        }

        #region IQueries

        public DesignerViewModel GetDesignerViewModel(DesignerKey designerKey, string languageName)
        {
            var cacheKey = String.Format(CultureInfo.InvariantCulture, DesignerViewModelCacheKey_,
                designerKey.ToString(), languageName);
            return _queryCache.GetOrInsert(cacheKey,
                () => _inner.GetDesignerViewModel(designerKey, languageName));
        }

        public IEnumerable<PatternViewItem> GetHomeViewModel()
        {
            return _queryCache.GetOrInsert(HomeCacheKey_, () => _inner.GetHomeViewModel());
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey)
        {
            var cacheKey = String.Format(CultureInfo.InvariantCulture, PatternCacheKey_, designerKey.ToString());
            return _queryCache.GetOrInsert(cacheKey, () => _inner.ListPatterns(designerKey));
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey)
        {
            return from _ in ListPatterns(designerKey) where _.CategoryKey == categoryKey select _;
        }

        public Pattern GetPattern(DesignerKey designerKey, string reference)
        {
            return (from _ in ListPatterns(designerKey) where _.Reference == reference select _).SingleOrDefault();
        }

        #endregion
    }
}
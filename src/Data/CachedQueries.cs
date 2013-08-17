namespace Chiffon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;

    public class CachedQueries : IQueries
    {
        readonly IQueries _inner;
        readonly IChiffonCacher _cacher;

        public CachedQueries(IQueries inner, IChiffonCacher cacher)
        {
            Requires.NotNull(inner, "inner");
            Requires.NotNull(cacher, "cacher");

            _inner = inner;
            _cacher = cacher;
        }

        #region IQueries

        public DesignerViewModel GetDesignerViewModel(DesignerKey designerKey, string languageName)
        {
            var format = ChiffonCacheKeyRegistry.GetCacheFormat(ChiffonCacheKey.GetDesignerViewModelQuery);
            var cacheKey = String.Format(CultureInfo.InvariantCulture, format, designerKey.ToString(), languageName);
            return _cacher.GetOrInsert(cacheKey,
                () => _inner.GetDesignerViewModel(designerKey, languageName));
        }

        public IEnumerable<PatternViewItem> GetHomeViewModel()
        {
            var cacheKey = ChiffonCacheKeyRegistry.GetCacheFormat(ChiffonCacheKey.GetHomeViewModelQuery);
            return _cacher.GetOrInsert(cacheKey, () => _inner.GetHomeViewModel());
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey)
        {
            var format = ChiffonCacheKeyRegistry.GetCacheFormat(ChiffonCacheKey.ListPatternsQuery);
            var cacheKey = String.Format(CultureInfo.InvariantCulture, format, designerKey.ToString());
            return _cacher.GetOrInsert(cacheKey, () => _inner.ListPatterns(designerKey));
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
namespace Chiffon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Fx;

    public class CachedViewModelQueries : IViewModelQueries
    {
        const string CategoryCacheKey_ = "Chiffon:Category:{0}:{1}:{2}";
        const string DesignerCacheKey_ = "Chiffon:Designer:{0}:{1}";
        const string HomeCacheKey_ = "Chiffon:Home";
        const string PatternCacheKey_ = "Chiffon:Pattern:{0}:{1}:{2}:{3}";

        readonly IViewModelQueries _inner;
        readonly IQueryCache _queryCache;

        public CachedViewModelQueries(IViewModelQueries inner, IQueryCache queryCache)
        {
            Requires.NotNull(inner, "inner");
            Requires.NotNull(queryCache, "queryCache");

            _inner = inner;
            _queryCache = queryCache;
        }

        #region IViewModelQueries

        public IEnumerable<PatternViewItem> GetHomeViewModel()
        {
            return _queryCache.GetOrInsert(HomeCacheKey_, () => _inner.GetHomeViewModel());
        }

        public Maybe<CategoryViewModel> MayGetCategoryViewModel(
            DesignerKey designerKey, string categoryKey, string languageName)
        {
            throw new NotImplementedException();
            //var cacheKey = GetCategoryKey_(designerKey, categoryKey, languageName);
            //return _queryCache.GetOrInsert(cacheKey,
            //    () => _inner.MayGetCategoryViewModel(designerKey,categoryKey, languageName));
        }

        public DesignerViewModel GetDesignerViewModel(DesignerKey designerKey, string languageName)
        {
            var cacheKey = GetDesignerKey_(designerKey, languageName);
            return _queryCache.GetOrInsert(cacheKey, () => _inner.GetDesignerViewModel(designerKey, languageName));
        }

        public Maybe<CategoryViewModel> MayGetPatternViewModel(
            DesignerKey designerKey, string categoryKey, string reference, string languageName)
        {
            throw new NotImplementedException();
            //var cacheKey = GetPatternKey_(designerKey, categoryKey, reference, languageName);
            //return _queryCache.GetOrInsert(cacheKey,
            //    () => _inner.MayGetPatternViewModel(designerKey,categoryKey, reference, languageName));
        }

        #endregion

        static string GetCategoryKey_(DesignerKey designerKey, string categoryKey, string languageName)
        {
            return String.Format(CultureInfo.InvariantCulture, CategoryCacheKey_,
                designerKey.ToString(), categoryKey, languageName);
        }

        static string GetDesignerKey_(DesignerKey designerKey, string languageName)
        {
            return String.Format(CultureInfo.InvariantCulture, DesignerCacheKey_,
                designerKey.ToString(), languageName);
        }

        static string GetPatternKey_(DesignerKey designerKey, string categoryKey, string reference, string languageName)
        {
            return String.Format(CultureInfo.InvariantCulture, PatternCacheKey_,
                designerKey.ToString(), categoryKey, reference, languageName);
        }
    }
}
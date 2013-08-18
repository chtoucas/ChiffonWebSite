namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo;

    public class CachedQueries : IQueries
    {
        readonly IQueries _inner;
        readonly IQueryCache _cache;

        public CachedQueries(IQueries inner, IQueryCache cache)
        {
            Requires.NotNull(inner, "inner");
            Requires.NotNull(cache, "cache");

            _inner = inner;
            _cache = cache;
        }

        #region IQueries

        public IEnumerable<PatternViewItem> GetHomeViewModel()
        {
            return _cache.GetOrInsertHomeViewModel(() => _inner.GetHomeViewModel());
        }

        public Designer GetDesigner(DesignerKey designerKey, string languageName)
        {
            return (from _ in ListDesigners(languageName) where _.Key == designerKey select _).SingleOrDefault();
        }

        public Pattern GetPattern(DesignerKey designerKey, string reference)
        {
            return (from _ in ListPatterns(designerKey) where _.Reference == reference select _).SingleOrDefault();
        }

        public IEnumerable<Category> ListCategories(DesignerKey designerKey, string languageName)
        {
            return _cache.GetOrInsertCategories(designerKey, languageName, (a, b) => _inner.ListCategories(a, b));
        }

        public IEnumerable<Designer> ListDesigners(string languageName)
        {
            return _cache.GetOrInsertDesigners(languageName, _ => _inner.ListDesigners(_));
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey)
        {
            return _cache.GetOrInsertPatterns(designerKey, _ => _inner.ListPatterns(_));
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey)
        {
            return from _ in ListPatterns(designerKey) where _.CategoryKey == categoryKey select _;
        }

        #endregion
    }
}
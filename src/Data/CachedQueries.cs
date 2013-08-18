namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;

    public class CachedQueries : IQueries
    {
        readonly IQueries _inner;
        readonly IChiffonCache _cacher;

        public CachedQueries(IQueries inner, IChiffonCache cacher)
        {
            Requires.NotNull(inner, "inner");
            Requires.NotNull(cacher, "cacher");

            _inner = inner;
            _cacher = cacher;
        }

        #region IQueries

        public DesignerViewModel GetDesignerViewModel(DesignerKey designerKey, string languageName)
        {
            return _cacher.GetOrInsertDesignerViewModel(designerKey, languageName, (a, b) => _inner.GetDesignerViewModel(a, b));
        }

        public IEnumerable<PatternViewItem> GetHomeViewModel()
        {
            return _cacher.GetOrInsertHomeViewModel(() => _inner.GetHomeViewModel());
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey)
        {
            return _cacher.GetOrInsertPatterns(designerKey, _ => _inner.ListPatterns(_));
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
namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
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

        public IEnumerable<Pattern> ListShowcasedPatterns()
        {
            return _cache.GetOrInsertShowcasedPatterns(() => _inner.ListShowcasedPatterns());
        }

        public Designer GetDesigner(DesignerKey designerKey, CultureInfo culture)
        {
            return (from _ in ListDesigners(culture) where _.Key == designerKey select _).SingleOrDefault();
        }

        public Pattern GetPattern(DesignerKey designerKey, string reference, string version)
        {
            return (from _ in ListPatterns(designerKey) 
                    where _.Reference == reference && _.Variant == version 
                    select _).SingleOrDefault();
        }

        public IEnumerable<Category> ListCategories(DesignerKey designerKey)
        {
            return _cache.GetOrInsertCategories(designerKey, _ => _inner.ListCategories(_));
        }

        public IEnumerable<Designer> ListDesigners(CultureInfo culture)
        {
            return _cache.GetOrInsertDesigners(culture, _ => _inner.ListDesigners(_));
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey)
        {
            return _cache.GetOrInsertPatterns(designerKey, _ => _inner.ListPatterns(_));
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey)
        {
            return from _ in ListPatterns(designerKey) where _.CategoryKey == categoryKey select _;
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey, bool published)
        {
            return from _ in ListPatterns(designerKey)
                   where _.CategoryKey == categoryKey && _.Published
                   select _;
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, bool published)
        {
            return from _ in ListPatterns(designerKey) where _.Published select _;
        }

        #endregion
    }
}
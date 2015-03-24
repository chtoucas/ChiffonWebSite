namespace Chiffon.Persistence
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Chiffon.Entities;
    using Narvalo;

    public class CachedDbQueries : IDbQueries
    {
        readonly IDbQueries _inner;
        readonly IDbQueryCache _cache;

        public CachedDbQueries(IDbQueries inner, IDbQueryCache cache)
        {
            Require.NotNull(inner, "inner");
            Require.NotNull(cache, "cache");

            _inner = inner;
            _cache = cache;
        }

        #region IDbQueries

        public Designer GetDesigner(DesignerKey designerKey, CultureInfo culture)
        {
            return (from _ in ListDesigners(culture) where _.Key == designerKey select _).SingleOrDefault();
        }

        public Member GetMember(string email, string password)
        {
            return _inner.GetMember(email, password);
        }

        public string GetPassword(string email)
        {
            return _inner.GetPassword(email);
        }

        public Pattern GetPattern(DesignerKey designerKey, string reference, string variant)
        {
            return (from _ in ListPatterns(designerKey)
                    where _.Reference == reference && _.Variant == variant
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

        public IEnumerable<Pattern> ListShowcasedPatterns()
        {
            return _cache.GetOrInsertShowcasedPatterns(() => _inner.ListShowcasedPatterns());
        }

        #endregion
    }
}
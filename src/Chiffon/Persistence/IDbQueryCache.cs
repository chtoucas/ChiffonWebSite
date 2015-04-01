namespace Chiffon.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Chiffon.Entities;

    public interface IDbQueryCache
    {
        IEnumerable<Category> GetOrInsertCategories(
            DesignerKey designerKey,
            Func<DesignerKey, IEnumerable<Category>> query);

        IEnumerable<Designer> GetOrInsertDesigners(
            CultureInfo culture,
            Func<CultureInfo, IEnumerable<Designer>> query);

        IEnumerable<Pattern> GetOrInsertPatterns(
            DesignerKey designerKey,
            Func<DesignerKey, IEnumerable<Pattern>> query);

        IEnumerable<Pattern> GetOrInsertShowcasedPatterns(Func<IEnumerable<Pattern>> query);
    }
}
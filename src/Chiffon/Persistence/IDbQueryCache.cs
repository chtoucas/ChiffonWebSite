namespace Chiffon.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Chiffon.Entities;

    public interface IDbQueryCache
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IEnumerable<Category> GetOrInsertCategories(
            DesignerKey designerKey,
            Func<DesignerKey, IEnumerable<Category>> query);

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IEnumerable<Designer> GetOrInsertDesigners(
            CultureInfo culture,
            Func<CultureInfo, IEnumerable<Designer>> query);

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IEnumerable<Pattern> GetOrInsertPatterns(
            DesignerKey designerKey,
            Func<DesignerKey, IEnumerable<Pattern>> query);

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IEnumerable<Pattern> GetOrInsertShowcasedPatterns(Func<IEnumerable<Pattern>> query);
    }
}
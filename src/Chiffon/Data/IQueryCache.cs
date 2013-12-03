namespace Chiffon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;

    public interface IQueryCache
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IEnumerable<Pattern> GetOrInsertShowcasedPatterns(Func<IEnumerable<Pattern>> query);

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IEnumerable<Category> GetOrInsertCategories(DesignerKey designerKey,
            Func<DesignerKey, IEnumerable<Category>> query);

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IEnumerable<Designer> GetOrInsertDesigners(ChiffonCulture culture,
            Func<ChiffonCulture, IEnumerable<Designer>> query);

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IEnumerable<Pattern> GetOrInsertPatterns(DesignerKey designerKey, 
            Func<DesignerKey, IEnumerable<Pattern>> query);
    }
}
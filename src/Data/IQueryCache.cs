namespace Chiffon.Data
{
    using System;
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;

    public interface IQueryCache
    {
        IEnumerable<PatternViewItem> GetOrInsertHomeViewModel(
            Func<IEnumerable<PatternViewItem>> query);

        IEnumerable<Category> GetOrInsertCategories(
            DesignerKey designerKey,
            Func<DesignerKey, IEnumerable<Category>> query);

        IEnumerable<Designer> GetOrInsertDesigners(ChiffonCulture culture,
            Func<ChiffonCulture, IEnumerable<Designer>> query);

        IEnumerable<Pattern> GetOrInsertPatterns(
            DesignerKey designerKey, Func<DesignerKey, IEnumerable<Pattern>> query);
    }
}
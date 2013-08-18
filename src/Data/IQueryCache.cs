namespace Chiffon.Data
{
    using System;
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.ViewModels;

    public interface IQueryCache
    {
        IEnumerable<PatternViewItem> GetOrInsertHomeViewModel(Func<IEnumerable<PatternViewItem>> query);

        IEnumerable<Category> GetOrInsertCategories(
            DesignerKey designerKey, string languageName, Func<DesignerKey, String, IEnumerable<Category>> query);

        IEnumerable<Designer> GetOrInsertDesigners(string languageName, Func<String, IEnumerable<Designer>> query);

        IEnumerable<Pattern> GetOrInsertPatterns(
            DesignerKey designerKey, Func<DesignerKey, IEnumerable<Pattern>> query);
    }
}
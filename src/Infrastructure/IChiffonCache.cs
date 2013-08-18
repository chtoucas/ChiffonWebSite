namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.ViewModels;

    public interface IChiffonCache
    {
        DesignerViewModel GetOrInsertDesignerViewModel(
            DesignerKey designerKey, string languageName, Func<DesignerKey, string, DesignerViewModel> query);
        IEnumerable<PatternViewItem> GetOrInsertHomeViewModel(Func<IEnumerable<PatternViewItem>> query);
        IEnumerable<Category> GetOrInsertCategories(
            DesignerKey designerKey, Func<DesignerKey, IEnumerable<Category>> query);
        IEnumerable<Pattern> GetOrInsertPatterns(
            DesignerKey designerKey, Func<DesignerKey, IEnumerable<Pattern>> query);
    }
}
namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.ViewModels;

    public interface IChiffonCache
    {
        DesignerViewModel GetDesignerViewModel(DesignerKey designerKey, string languageName, Func<DesignerKey, string, DesignerViewModel> query);
        IEnumerable<PatternViewItem> GetHomeViewModel(Func<IEnumerable<PatternViewItem>> query);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, Func<DesignerKey, IEnumerable<Pattern>> query);
    }
}
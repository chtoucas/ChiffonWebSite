namespace Chiffon.Data
{
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.ViewModels;

    public interface IQueries
    {
        DesignerViewModel GetDesignerViewModel(DesignerKey designerKey, string languageName);
        IEnumerable<PatternViewItem> GetHomeViewModel();

        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey);

        Pattern GetPattern(DesignerKey designerKey, string reference);
    }
}

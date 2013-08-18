namespace Chiffon.Data
{
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.ViewModels;

    public interface IQueries
    {
        IEnumerable<PatternViewItem> GetHomeViewModel();

        IEnumerable<Category> ListCategories(DesignerKey designerKey, string languageName);
        IEnumerable<Designer> ListDesigners(string languageName);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey);

        Designer GetDesigner(DesignerKey designerKey, string languageName);
        Pattern GetPattern(DesignerKey designerKey, string reference);
    }
}

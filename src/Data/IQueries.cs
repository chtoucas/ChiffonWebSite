namespace Chiffon.Data
{
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;

    public interface IQueries
    {
        IEnumerable<PatternViewItem> GetHomeViewModel();

        IEnumerable<Category> ListCategories(DesignerKey designerKey);
        IEnumerable<Designer> ListDesigners(ChiffonCulture culture);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey);

        Designer GetDesigner(DesignerKey designerKey, ChiffonCulture culture);
        Pattern GetPattern(DesignerKey designerKey, string reference);
    }
}

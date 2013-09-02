namespace Chiffon.Data
{
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;

    public interface IQueries
    {
        IEnumerable<Category> ListCategories(DesignerKey designerKey);
        IEnumerable<Designer> ListDesigners(ChiffonCulture culture);

        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey, bool published);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, bool published);
        IEnumerable<Pattern> ListShowcasedPatterns();

        Designer GetDesigner(DesignerKey designerKey, ChiffonCulture culture);
        Pattern GetPattern(DesignerKey designerKey, string reference, string version);
    }
}

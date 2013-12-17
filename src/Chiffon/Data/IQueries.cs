namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;

    public interface IQueries
    {
        IEnumerable<Category> ListCategories(DesignerKey designerKey);
        IEnumerable<Designer> ListDesigners(CultureInfo culture);

        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey, bool published);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, bool published);
        IEnumerable<Pattern> ListShowcasedPatterns();

        Designer GetDesigner(DesignerKey designerKey, CultureInfo culture);
        Pattern GetPattern(DesignerKey designerKey, string reference, string version);
    }
}

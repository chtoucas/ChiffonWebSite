namespace Chiffon.Persistence
{
    using System.Collections.Generic;
    using System.Globalization;
    using Chiffon.Entities;

    /// <summary>
    /// Représente l'ensemble des opérations permettant l'accès en lecture au stockage persistant.
    /// </summary>
    public interface IDbQueries
    {
        Designer GetDesigner(DesignerKey designerKey, CultureInfo culture);
        Pattern GetPattern(DesignerKey designerKey, string reference, string variant);

        Member GetMember(string email, string password);
        string GetPassword(string email);

        IEnumerable<Category> ListCategories(DesignerKey designerKey);
        IEnumerable<Designer> ListDesigners(CultureInfo culture);

        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey, bool published);
        IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, bool published);

        IEnumerable<Pattern> ListShowcasedPatterns();
    }
}

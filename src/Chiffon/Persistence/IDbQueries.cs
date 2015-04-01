namespace Chiffon.Persistence
{
    using System.Collections.Generic;
    using System.Globalization;

    using Chiffon.Entities;

    /// <summary>
    /// Représente l'ensemble des opérations permettant l'accès en lecture au stockage persistant.
    /// </summary>
    public partial interface IDbQueries
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

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Chiffon.Persistence
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;

    using Chiffon.Entities;

    [ContractClass(typeof(IDbQueriesContract))]
    public partial interface IDbQueries { }

    [ContractClassFor(typeof(IDbQueries))]
    internal abstract class IDbQueriesContract : IDbQueries
    {
        Designer IDbQueries.GetDesigner(DesignerKey designerKey, CultureInfo culture)
        {
            return default(Designer);
        }

        Pattern IDbQueries.GetPattern(DesignerKey designerKey, string reference, string variant)
        {
            return default(Pattern);
        }

        Member IDbQueries.GetMember(string email, string password)
        {
            return default(Member);
        }

        string IDbQueries.GetPassword(string email)
        {
            return default(string);
        }

        IEnumerable<Category> IDbQueries.ListCategories(DesignerKey designerKey)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Category>>() != null);

            return Enumerable.Empty<Category>();
        }

        IEnumerable<Designer> IDbQueries.ListDesigners(CultureInfo culture)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Designer>>() != null);

            return Enumerable.Empty<Designer>();
        }

        IEnumerable<Pattern> IDbQueries.ListPatterns(DesignerKey designerKey)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Pattern>>() != null);

            return Enumerable.Empty<Pattern>();
        }

        IEnumerable<Pattern> IDbQueries.ListPatterns(DesignerKey designerKey, string categoryKey)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Pattern>>() != null);

            return Enumerable.Empty<Pattern>();
        }

        IEnumerable<Pattern> IDbQueries.ListPatterns(DesignerKey designerKey, string categoryKey, bool published)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Pattern>>() != null);

            return Enumerable.Empty<Pattern>();
        }

        IEnumerable<Pattern> IDbQueries.ListPatterns(DesignerKey designerKey, bool published)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Pattern>>() != null);

            return Enumerable.Empty<Pattern>();
        }

        IEnumerable<Pattern> IDbQueries.ListShowcasedPatterns()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Pattern>>() != null);

            return Enumerable.Empty<Pattern>();
        }
    }
}

#endif

namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using Chiffon.Data.SqlServer;
    using Chiffon.Entities;
    using Narvalo;

    /// <summary>
    /// Implémentation standard de <see cref="Chiffon.Data.IQueries"/>.
    /// </summary>
    public class Queries : IQueries
    {
        readonly string _connectionString;

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Chiffon.Data.Queries"/>.
        /// </summary>
        /// <param name="connectionString">Chaîne de connexion à la base de données.</param>
        /// <exception cref="System.ArgumentNullException">connectionString est null.</exception>
        /// <exception cref="System.ArgumentException">connectionString est une chaîne vide.</exception>
        public Queries(string connectionString)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");

            _connectionString = connectionString;
        }

        /// <summary>
        /// Retourne la chaîne de connexion à la base de données tel que spécifiée 
        /// lors de la création de l'objet.
        /// </summary>
        protected string ConnectionString { get { return _connectionString; } }

        #region IQueries

        public Designer GetDesigner(DesignerKey designerKey, CultureInfo culture)
        {
            var q = new GetDesignerQuery(ConnectionString, designerKey, culture);
            return q.Execute();
        }

        public Member GetMember(string email, string password)
        {
            var q = new GetMemberQuery(ConnectionString, email, password);
            return q.Execute();
        }

        public string GetPassword(string email)
        {
            var q = new GetPasswordQuery(ConnectionString, email);
            return q.Execute();
        }

        public Pattern GetPattern(DesignerKey designerKey, string reference, string variant)
        {
            var q = new GetPatternQuery(ConnectionString, designerKey, reference, variant);
            return q.Execute();
        }

        public IEnumerable<Category> ListCategories(DesignerKey designerKey)
        {
            var q = new ListCategoriesQuery(ConnectionString, designerKey);
            return q.Execute();
        }

        public IEnumerable<Designer> ListDesigners(CultureInfo culture)
        {
            var q = new ListDesignersQuery(ConnectionString, culture);
            return q.Execute();
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey)
        {
            var q = new ListPatternsQuery(ConnectionString, designerKey);
            return q.Execute();
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey)
        {
            var q = new ListPatternsQuery(ConnectionString, designerKey) { CategoryKey = categoryKey };
            return q.Execute();
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey, bool published)
        {
            var q = new ListPatternsQuery(ConnectionString, designerKey) {
                CategoryKey = categoryKey,
                Published = published,
            };
            return q.Execute();
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, bool published)
        {
            var q = new ListPatternsQuery(ConnectionString, designerKey) { Published = published };
            return q.Execute();
        }

        public IEnumerable<Pattern> ListShowcasedPatterns()
        {
            var q = new ListShowcasedPatternsQuery(ConnectionString);
            return q.Execute();
        }

        #endregion
    }
}

﻿namespace Chiffon.Persistence
{
    using System.Collections.Generic;
    using System.Globalization;
    using Chiffon.Persistence.SqlServer;
    using Chiffon.Domain;
    using Chiffon.Entities;
    using Narvalo;

    /// <summary>
    /// Implémentation standard de <see cref="Chiffon.Persistence.IDbQueries"/>.
    /// </summary>
    public class DbQueries : IDbQueries
    {
        readonly string _connectionString;

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Chiffon.Persistence.DbQueries"/>.
        /// </summary>
        /// <param name="connectionString">Chaîne de connexion à la base de données.</param>
        /// <exception cref="System.ArgumentNullException">connectionString est null.</exception>
        /// <exception cref="System.ArgumentException">connectionString est une chaîne vide.</exception>
        public DbQueries(string connectionString)
        {
            Require.NotNullOrEmpty(connectionString, "connectionString");

            _connectionString = connectionString;
        }

        /// <summary>
        /// Retourne la chaîne de connexion à la base de données tel que spécifiée 
        /// lors de la création de l'objet.
        /// </summary>
        protected string ConnectionString { get { return _connectionString; } }

        #region IDbQueries

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

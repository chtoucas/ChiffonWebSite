namespace Chiffon.Data
{
    using Chiffon.Data.SqlServer;
    using Chiffon.Entities;
    using Narvalo;

    /// <summary>
    /// Implémentation standard de <see cref="Chiffon.Data.IWriteQueries"/>.
    /// </summary>
    public class WriteQueries : IWriteQueries
    {
        readonly string _connectionString;

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Chiffon.Data.WriteQueries"/>.
        /// </summary>
        /// <param name="connectionString">Chaîne de connexion à la base de données.</param>
        /// <exception cref="System.ArgumentNullException">connectionString est null.</exception>
        /// <exception cref="System.ArgumentException">connectionString est une chaîne vide.</exception>
        public WriteQueries(string connectionString)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");

            _connectionString = connectionString;
        }

        /// <summary>
        /// Retourne la chaîne de connexion à la base de données tel que spécifiée
        /// lors de la création de l'objet.
        /// </summary>
        protected string ConnectionString { get { return _connectionString; } }

        #region IWriteQueries

        public Member NewMember(NewMemberParameters parameters)
        {
            Requires.NotNull(parameters, "parameters");

            var q = new NewMemberQuery(ConnectionString);
            q.Execute(parameters);

            return new Member {
                Email = parameters.Email,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
            };
        }

        #endregion
    }

}

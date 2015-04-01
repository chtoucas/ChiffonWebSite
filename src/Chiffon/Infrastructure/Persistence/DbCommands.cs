namespace Chiffon.Infrastructure.Persistence
{
    using Chiffon.Infrastructure.Persistence.SqlServer;
    using Narvalo;

    /// <summary>
    /// Implémentation standard de <see cref="Chiffon.Infrastructure.Persistence.IDbCommands"/>.
    /// </summary>
    public class DbCommands : IDbCommands
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Chiffon.Infrastructure.Persistence.DbCommands"/>.
        /// </summary>
        /// <param name="connectionString">Chaîne de connexion à la base de données.</param>
        /// <exception cref="System.ArgumentNullException">connectionString est null.</exception>
        /// <exception cref="System.ArgumentException">connectionString est une chaîne vide.</exception>
        public DbCommands(string connectionString)
        {
            Require.NotNullOrEmpty(connectionString, "connectionString");

            _connectionString = connectionString;
        }

        /// <summary>
        /// Retourne la chaîne de connexion à la base de données tel que spécifiée
        /// lors de la création de l'objet.
        /// </summary>
        protected string ConnectionString { get { return _connectionString; } }

        public void NewMember(NewMemberParameters parameters)
        {
            Require.NotNull(parameters, "parameters");

            var q = new NewMemberCommand(ConnectionString);
            q.Execute(parameters);
        }
    }
}

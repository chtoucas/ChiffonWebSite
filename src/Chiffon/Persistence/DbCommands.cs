namespace Chiffon.Persistence
{
    using Chiffon.Persistence.SqlServer;
    using Narvalo;

    /// <summary>
    /// Implémentation standard de <see cref="Chiffon.Persistence.IDbCommands"/>.
    /// </summary>
    public sealed class DbCommands : IDbCommands
    {
        /// <summary>
        /// Chaîne de connexion à la base de données tel que spécifiée lors de la création de l'objet.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Chiffon.Persistence.DbCommands"/>.
        /// </summary>
        /// <param name="connectionString">Chaîne de connexion à la base de données.</param>
        /// <exception cref="System.ArgumentNullException">connectionString est null.</exception>
        /// <exception cref="System.ArgumentException">connectionString est une chaîne vide.</exception>
        public DbCommands(string connectionString)
        {
            Require.NotNullOrEmpty(connectionString, "connectionString");

            _connectionString = connectionString;
        }

        public void NewMember(NewMemberParameters parameters)
        {
            Require.NotNull(parameters, "parameters");

            var q = new NewMemberCommand(_connectionString);
            q.Execute(parameters);
        }
    }
}

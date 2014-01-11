namespace Chiffon.Data
{
    using Chiffon.Data.SqlServer;
    using Chiffon.Entities;
    using Narvalo;

    /// <summary>
    /// Implémentation standard de <see cref="Chiffon.Data.ICommands"/>.
    /// </summary>
    public class Commands : ICommands
    {
        readonly string _connectionString;

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Chiffon.Data.Commands"/>.
        /// </summary>
        /// <param name="connectionString">Chaîne de connexion à la base de données.</param>
        /// <exception cref="System.ArgumentNullException">connectionString est null.</exception>
        /// <exception cref="System.ArgumentException">connectionString est une chaîne vide.</exception>
        public Commands(string connectionString)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");

            _connectionString = connectionString;
        }

        /// <summary>
        /// Retourne la chaîne de connexion à la base de données tel que spécifiée
        /// lors de la création de l'objet.
        /// </summary>
        protected string ConnectionString { get { return _connectionString; } }

        #region ICommands

        public void NewMember(NewMemberParameters parameters)
        {
            Requires.NotNull(parameters, "parameters");

            var q = new NewMemberCommand(ConnectionString);
            q.Execute(parameters);
        }

        #endregion
    }

}

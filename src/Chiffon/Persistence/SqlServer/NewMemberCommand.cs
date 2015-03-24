namespace Chiffon.Persistence.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;

    using Narvalo;
    using Narvalo.Data;

    /// <summary>
    /// Représente la procédure stockée "usp_NewMember".
    /// </summary>
    public class NewMemberCommand : NonQueryStoredProcedure<NewMemberParameters>
    {
        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Chiffon.Persistence.SqlServer.NewMemberCommand"/>.
        /// </summary>
        /// <param name="connectionString">Chaîne de connexion à la base de données.</param>
        /// <exception cref="System.ArgumentNullException">connectionString est null.</exception>
        /// <exception cref="System.ArgumentException">connectionString est une chaîne de caractères vide.</exception>
        public NewMemberCommand(string connectionString)
            : base(connectionString, "usp_NewMember") { }

        protected override void AddParameters(SqlParameterCollection parameters, NewMemberParameters values)
        {
            Require.NotNull(parameters, "parameters");
            Require.NotNull(values, "values");

            parameters.AddParameterUnsafe("@email_address", SqlDbType.NVarChar, values.Email);
            parameters.AddParameterUnsafe("@firstname", SqlDbType.NVarChar, values.FirstName);
            parameters.AddParameterUnsafe("@lastname", SqlDbType.NVarChar, values.LastName);
            parameters.AddParameterUnsafe("@company_name", SqlDbType.NVarChar, values.CompanyName);
            parameters.AddParameterUnsafe("@password", SqlDbType.NVarChar, values.EncryptedPassword);
            parameters.AddParameterUnsafe("@newsletter", SqlDbType.Bit, values.NewsletterChecked);
        }
    }
}
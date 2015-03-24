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

            parameters.Add("@email_address", SqlDbType.NVarChar).Value = values.Email;
            parameters.Add("@firstname", SqlDbType.NVarChar).Value = values.FirstName;
            parameters.Add("@lastname", SqlDbType.NVarChar).Value = values.LastName;
            parameters.Add("@company_name", SqlDbType.NVarChar).Value = values.CompanyName;
            parameters.Add("@password", SqlDbType.NVarChar).Value = values.EncryptedPassword;
            parameters.Add("@newsletter", SqlDbType.Bit).Value = values.NewsletterChecked;
        }
    }
}
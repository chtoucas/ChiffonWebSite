namespace Chiffon.Persistence.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;
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

        protected override void PrepareCommand(SqlCommand command, NewMemberParameters parameters)
        {
            SqlParameterCollection p = command.Parameters;
            p.Add("@email_address", SqlDbType.NVarChar).Value = parameters.Email;
            p.Add("@firstname", SqlDbType.NVarChar).Value = parameters.FirstName;
            p.Add("@lastname", SqlDbType.NVarChar).Value = parameters.LastName;
            p.Add("@company_name", SqlDbType.NVarChar).Value = parameters.CompanyName;
            p.Add("@password", SqlDbType.NVarChar).Value = parameters.EncryptedPassword;
            p.Add("@newsletter", SqlDbType.Bit).Value = parameters.NewsletterChecked;
        }
    }
}
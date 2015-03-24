namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Narvalo;
    using Narvalo.Data;

    public class GetPasswordQuery : StoredProcedure<String>
    {
        public GetPasswordQuery(string connectionString, string email)
            : base(connectionString, "usp_GetPublicKeyByEmailAddress")
        {
            Email = email;

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        public string Email { get; private set; }

        protected override string Execute(SqlDataReader rdr)
        {
            Require.NotNull(rdr, "rdr");

            if (!rdr.Read()) { return null; }

            return rdr.GetString("public_key");
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            parameters.AddParameter("@email_address", SqlDbType.NVarChar, Email);
        }
    }
}
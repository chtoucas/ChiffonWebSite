namespace Chiffon.Persistence.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;

    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public class GetMemberQuery : StoredProcedure<Member>
    {
        public GetMemberQuery(string connectionString, string email, string password)
            : base(connectionString, "usp_GetContactByPublicKey")
        {
            Email = email;
            Password = password;

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        public string Email { get; private set; }
        public string Password { get; private set; }

        protected override Member Execute(SqlDataReader rdr)
        {
            Require.NotNull(rdr, "rdr");

            if (!rdr.Read()) { return null; }

            return MemberFactory.NewMember(Email, rdr.GetString("firstname"), rdr.GetString("lastname"));
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            parameters.AddParameter("@email_address", SqlDbType.NVarChar, Email);
            parameters.AddParameter("@password", SqlDbType.NVarChar, Password);
        }
    }
}
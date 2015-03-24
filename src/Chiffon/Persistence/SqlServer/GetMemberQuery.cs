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

        protected override Member Execute(SqlDataReader reader)
        {
            Require.NotNull(reader, "reader");

            if (!reader.Read()) { return null; }

            return MemberFactory.NewMember(
                Email, 
                reader.GetStringUnsafe("firstname"),
                reader.GetStringUnsafe("lastname"));
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Require.NotNull(parameters, "parameters");

            parameters.AddParameterUnsafe("@email_address", SqlDbType.NVarChar, Email);
            parameters.AddParameterUnsafe("@password", SqlDbType.NVarChar, Password);
        }
    }
}
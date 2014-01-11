namespace Chiffon.Data.SqlServer
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
            Requires.NotNull(rdr, "rdr");

            if (!rdr.Read()) { return null; }

            return new Member {
                Email = Email,
                FirstName = rdr.GetString("firstname"),
                LastName = rdr.GetString("lastname"),
            };
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@email_address", SqlDbType.NVarChar, Email);
            command.AddParameter("@password", SqlDbType.NVarChar, Password);
        }
    }
}
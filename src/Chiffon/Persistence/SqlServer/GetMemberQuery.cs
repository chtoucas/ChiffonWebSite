namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    using Chiffon.Entities;
    using Chiffon.Internal;
    using Narvalo;
    using Narvalo.Data;

    public sealed class GetMemberQuery : StoredProcedure<Member>
    {
        public GetMemberQuery(string connectionString, string email, string password)
            : base(connectionString, "usp_GetContactByPublicKey")
        {
            Contract.Requires(!String.IsNullOrEmpty(connectionString));

            Email = email;
            Password = password;

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        public string Email { get; private set; }

        public string Password { get; private set; }

        protected override Member Execute([ValidatedNotNull]SqlDataReader reader)
        {
            CheckFor.StoredProcedure.Execute(reader);

            if (!reader.Read()) { return null; }

            return MemberFactory.NewMember(
                Email, 
                reader.GetStringUnsafe("firstname"),
                reader.GetStringUnsafe("lastname"));
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Check.NotNull(parameters, "The base class guarantees that the parameter is not null.");

            parameters.AddParameterUnsafe("@email_address", SqlDbType.NVarChar, Email);
            parameters.AddParameterUnsafe("@password", SqlDbType.NVarChar, Password);
        }
    }
}
namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    using Narvalo;
    using Narvalo.Data;

    public sealed class GetPasswordQuery : StoredProcedure<String>
    {
        public GetPasswordQuery(string connectionString, string email)
            : base(connectionString, "usp_GetPublicKeyByEmailAddress")
        {
            Contract.Requires(!String.IsNullOrEmpty(connectionString));

            Email = email;

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        public string Email { get; private set; }

        protected override string Execute(SqlDataReader reader)
        {
            Check.NotNull(reader, "The base class guarantees that the parameter is not null.");

            if (!reader.Read()) { return null; }

            return reader.GetStringUnsafe("public_key");
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Check.NotNull(parameters, "The base class guarantees that the parameter is not null.");

            parameters.AddParameterUnsafe("@email_address", SqlDbType.NVarChar, Email);
        }
    }
}
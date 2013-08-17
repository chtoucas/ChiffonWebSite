namespace Chiffon.Data.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;
    using Narvalo;

    public abstract class StoredProcedure<TResult>
    {
        readonly string _connectionString;
        readonly string _name;

        public StoredProcedure(string connectionString, string name)
        {
            Requires.NotNull(connectionString, "connectionString");
            Requires.NotNull(name, "name");

            _connectionString = connectionString;
            _name = name;
        }

        protected string ConnectionString { get { return _connectionString; } }
        protected string Name { get { return _name; } }

        public abstract TResult Execute();

        protected virtual void PrepareCommand(SqlCommand command) { }

        protected SqlCommand CreateCommand(SqlConnection connection)
        {
            var cmd = new SqlCommand(Name, connection) { CommandType = CommandType.StoredProcedure };
            PrepareCommand(cmd);
            return cmd;
        }
    }
}
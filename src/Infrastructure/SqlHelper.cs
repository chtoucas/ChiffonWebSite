namespace Chiffon.Infrastructure
{
    using System.Data;
    using System.Data.SqlClient;

    public class SqlHelper
    {
        readonly ChiffonConfig _config;

        public SqlHelper(ChiffonConfig config)
        {
            _config = config;
        }

        protected string ConnectionString { get { return _config.SqlConnectionString; } }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static SqlCommand CreateStoredProcedure(string name, SqlConnection connection)
        {
            return new SqlCommand(name, connection) { CommandType = CommandType.StoredProcedure };
        }
    }
}

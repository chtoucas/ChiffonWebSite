namespace Chiffon.Infrastructure
{
    using System.Data.SqlClient;
    using Chiffon.Crosscuttings;

    public class SqlHelper
    {
        readonly ChiffonConnectionStrings _connectionStrings;

        public SqlHelper(ChiffonConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        protected string ConnectionString { get { return _connectionStrings.ConnectionString; } }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}

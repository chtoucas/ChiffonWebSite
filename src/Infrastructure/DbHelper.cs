namespace Chiffon.Infrastructure
{
    using System.Data.SqlClient;
    using Chiffon.Infrastructure;

    public class DbHelper
    {
        readonly ChiffonConfig _config;

        public DbHelper(ChiffonConfig config)
        {
            _config = config;
        }

        protected string ConnectionString { get { return _config.SqlConnectionString; } }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}

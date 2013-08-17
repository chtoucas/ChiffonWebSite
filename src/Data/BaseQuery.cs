namespace Chiffon.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using Narvalo;

    public class BaseQuery
    {
        readonly string _connectionString;

        public BaseQuery(string connectionString)
        {
            Requires.NotNull(connectionString, "connectionString");

            _connectionString = connectionString;
        }

        protected string ConnectionString { get { return _connectionString; } }

        protected static SqlCommand NewStoredProcedure(string name, SqlConnection connection)
        {
            return new SqlCommand(name, connection) { CommandType = CommandType.StoredProcedure };
        }
    }

    //internal class StoredProcedure : IDisposable
    //{
    //    bool _disposed = false;
    //    string _connectionString;
    //    SqlCommand _command;
    //    SqlConnection _connection;

    //    public StoredProcedure(ChiffonConfig config, string name)
    //        : this(config.SqlConnectionString, name) { }

    //    public StoredProcedure(string connectionString, string name)
    //    {
    //        Requires.NotNullOrEmpty(connectionString, "connectionString");
    //        Requires.NotNullOrEmpty(name, "name");
    //        _connectionString = connectionString;
    //        _command = new SqlCommand(name) { CommandType = CommandType.StoredProcedure, };
    //    }

    //    public StoredProcedure AddParameter(string parameterName, SqlDbType sqlDbType, object value)
    //    {
    //        _command.Parameters.Add(parameterName, sqlDbType).Value = value;
    //        return this;
    //    }

    //    public SqlDataReader Execute()
    //    {
    //        _connection = new SqlConnection(_connectionString);
    //        _command.Connection = _connection;
    //        _connection.Open();
    //        return _command.ExecuteReader(CommandBehavior.CloseConnection);
    //    }

    //    #region IDisposable

    //    public void Dispose()
    //    {
    //        Dispose(true /* disposing */);
    //        GC.SuppressFinalize(this);
    //    }

    //    #endregion

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!_disposed) {
    //            if (disposing) {
    //                if (_command != null) {
    //                    _command.Dispose();
    //                    _command = null;
    //                }
    //                if (_connection != null) {
    //                    _connection.Dispose();
    //                    _connection = null;
    //                }
    //            }
    //            _disposed = true;
    //        }
    //    }
    //}
}
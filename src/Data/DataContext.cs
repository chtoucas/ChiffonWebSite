namespace Chiffon.Data
{
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo.Fx;

    public class DataContext
    {
        readonly ChiffonConfig _config;

        public DataContext(ChiffonConfig config)
        {
            _config = config;
        }

        protected string ConnectionString { get { return _config.SqlConnectionString; } }

        public List<PatternItem> GetHomeView()
        {
            return new HomeIndexQuery(ConnectionString).Execute();
        }

        public Maybe<CategoryViewModel> MayGetCategoryView(
            DesignerKey designerKey, string categoryKey, string languageName)
        {
            return new DesignerCategoryQuery(ConnectionString)
                .Execute(designerKey, categoryKey, languageName);
        }

        public DesignerViewModel GetDesignerView(DesignerKey designerKey, string languageName)
        {
            return new DesignerIndexQuery(ConnectionString).Execute(designerKey, languageName);
        }

        public Maybe<CategoryViewModel> MayGetPatternView(
            DesignerKey designerKey,
            string categoryKey,
            string reference,
            string languageName)
        {
            return new DesignerPatternQuery(ConnectionString)
                .Execute(designerKey, categoryKey, reference, languageName);
        }

        public Maybe<Pattern> MayGetPattern(DesignerKey designerKey, string reference)
        {
            return new MayGetPatternQuery(ConnectionString).Execute(designerKey, reference);
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

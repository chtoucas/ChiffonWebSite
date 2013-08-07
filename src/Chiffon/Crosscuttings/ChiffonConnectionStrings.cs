namespace Chiffon.Crosscuttings
{
    using System;
    using System.Configuration;
    using System.Globalization;

    public class ChiffonConnectionStrings
    {
        const string ConnectionStringName = "SqlServer";

        string _connectionString;

        public ChiffonConnectionStrings()
        {
            LoadConfigurationAndInitialize(ConfigurationManager.ConnectionStrings);
        }

        public ChiffonConnectionStrings(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        protected void LoadConfigurationAndInitialize(ConnectionStringSettingsCollection connections)
        {
            _connectionString = GetValue_(connections, ConnectionStringName);
        }

        static string GetValue_(ConnectionStringSettingsCollection connections, string key)
        {
            ConnectionStringSettings connection = connections[key];

            if (connection == null) {
                throw new ConfigurationErrorsException(
                    String.Format(CultureInfo.InvariantCulture,
                        "The {0} connection is not defined in your config file!",
                        key));
            }
            else {
                return connection.ConnectionString;
            }
        }
    }
}

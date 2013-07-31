namespace Pacr.CrossCuttings.Configuration
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;

    public static class PacrConfigurationManager
    {
        private const string ConnectionStringPropertyName = "ConnectionString";
        private const string AsyncConnectionStringPropertyName = "AsyncConnectionString";
        //private const string ExternalConfigFilePropertyName = "ExternalConfigFile";
        //private const string UseExternalConfigPropertyName = "UseExternalConfig";

        private static readonly string _configurationDirectory
			= Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

        // TODO: isn't ReadOnly enough?
        private static Lazy<PacrConfigurationSection> _configuration
            = new Lazy<PacrConfigurationSection>(() => OpenConfig());
        private static Lazy<PacrConnectionStrings> _connectionStrings
            = new Lazy<PacrConnectionStrings>(() => ReadConnectionStrings());

        public static PacrConfigurationSection Configuration
        {
            get
            {
                return _configuration.Value;
            }
        }

        public static PacrConnectionStrings ConnectionStrings
        {
            get
            {
                return _connectionStrings.Value;
            }
        }

        public static PacrConfigurationSection OpenConfig()
        {
            return PacrConfigurationSection.GetSection();
        }

        public static PacrConfigurationSection OpenExternalConfig(string configFileName)
        {
            if (configFileName == null) {
                throw new ArgumentNullException("configFileName");
            }

            if (!Path.IsPathRooted(configFileName)) {
                configFileName = Path.Combine(_configurationDirectory, configFileName);
            }

            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configFileName;

            var appConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            var config = appConfig.GetSection(PacrConfigurationSection.PacrSectionName) as PacrConfigurationSection;

            if (config == null) {
                throw new ConfigurationErrorsException(
                        String.Format(CultureInfo.InvariantCulture,
                            "The <{0}> section is not defined in your config file!",
                            PacrConfigurationSection.PacrSectionName));
            }

            return config;
        }

        public static PacrConnectionStrings ReadConnectionStrings()
        {
            return ReadConnectionStrings(ConfigurationManager.ConnectionStrings);
        }

        private static PacrConnectionStrings ReadConnectionStrings(ConnectionStringSettingsCollection connections)
        {
            return new PacrConnectionStrings {
                AsyncConnectionString = ReadConnectionString(connections, AsyncConnectionStringPropertyName),
                ConnectionString = ReadConnectionString(connections, ConnectionStringPropertyName),
            };
        }

        private static string ReadConnectionString(ConnectionStringSettingsCollection connections, string key)
        {
            ConnectionStringSettings connection = connections[key];

            if (connection == null) {
                throw new ConfigurationErrorsException(
                    String.Format(CultureInfo.InvariantCulture,
                        "The {0} connection is not defined in your config file!", key));
            }
            else {
                return connection.ConnectionString;
            }
        }

        //public static PacrConfiguration OpenConfig() {
        //    var appSettings = ConfigurationManager.AppSettings;

        //    string useExternalConfigValue = appSettings[UseExternalConfigPropertyName];

        //    bool useExternalConfig = useExternalConfigValue != null
        //        && useExternalConfigValue == "true";

        //    PacrConfiguration config;

        //    if (useExternalConfig) {
        //        string configFile = appSettings[ExternalConfigFilePropertyName];

        //        if (String.IsNullOrEmpty(configFile)) {
        //            throw new ConfigurationErrorsException(
        //                String.Format(CultureInfo.InvariantCulture,
        //                    "The {0} property is not defined in your config file!",
        //                    ExternalConfigFilePropertyName));
        //        }

        //        config = OpenExternalConfig(configFile);
        //    }
        //    else {
        //        config = OpenAppConfig();
        //    }

        //    return config;
        //}
    }
}

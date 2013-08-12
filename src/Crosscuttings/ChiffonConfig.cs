﻿namespace Chiffon.Crosscuttings
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using Narvalo;
    using Narvalo.Collections;
    using Serilog.Events;

    public class ChiffonConfig
    {
        const string SettingPrefix_ = "chiffon.";
        const string SqlConnectionStringName_ = "SqlServer";

        bool _debugCss = false;
        bool _debugJs = false;

        public Uri BaseUri { get; set; }
        public bool DebugCss { get { return _debugCss; } set { _debugCss = value; } }
        public bool DebugJs { get { return _debugJs; } set { _debugJs = value; } }
        public LogEventLevel LoggerLevel { get; set; }
        public string LoggerName { get; set; }
        public string PatternDirectory { get; set; }
        public string SqlConnectionString { get; set; }

        public static ChiffonConfig FromConfiguration()
        {
            return (new ChiffonConfig()).Load();
        }

        public ChiffonConfig Load()
        {
            LoadSettings_(ConfigurationManager.AppSettings);
            LoadConnectionStrings_(ConfigurationManager.ConnectionStrings);

            return this;
        }

        void LoadSettings_(NameValueCollection settings)
        {
            var chiffonKeys = settings.AllKeys
                .Where(_ => _.StartsWith(SettingPrefix_, StringComparison.InvariantCultureIgnoreCase));

            var chiffonSettings = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);

            foreach (var key in chiffonKeys) {
                chiffonSettings[key] = settings[key];
            }

            Initialize_(chiffonSettings);
        }

        void LoadConnectionStrings_(ConnectionStringSettingsCollection connections)
        {
            ConnectionStringSettings connection = connections[SqlConnectionStringName_];

            if (connection == null) {
                throw new ConfigurationErrorsException(
                    String.Format(CultureInfo.InvariantCulture,
                        "The {0} connection is not defined in your config file!",
                        SqlConnectionStringName_));
            }

            SqlConnectionString = connection.ConnectionString;
        }

        void Initialize_(NameValueCollection nvc)
        {
            // > Paramètres obligatoires <

            BaseUri = nvc.MayParseValue("chiffon.baseUri", _ => MayParse.ToUri(_, UriKind.Absolute))
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon.baseUri'."));

            // TODO: validate this? Absolute and well-formed.
            PatternDirectory = nvc.MayGetValue("chiffon.patternDirectory")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon.patternDirectory'."));

            LoggerName = nvc.MayGetValue("chiffon.loggerName")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon.loggerName'."));

            LoggerLevel = nvc.MayParseValue("chiffon.loggerLevel", _ => MayParse.ToEnum<LogEventLevel>(_))
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon.loggerLevel'."));

            // > Paramètres optionels <

            DebugJs = nvc.MayParseValue("chiffon.debugJs", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(false);

            DebugCss = nvc.MayParseValue("chiffon.debugCss", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(false);
        }
    }
}

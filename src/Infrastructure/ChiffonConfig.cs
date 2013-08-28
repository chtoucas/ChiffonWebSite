﻿namespace Chiffon.Infrastructure
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
        const string SettingPrefix_ = "chiffon:";
        const string SqlConnectionStringName_ = "SqlServer";

        bool _debugCss = false;
        bool _debugJs = false;
        bool _enableClientCache = true;
        bool _enableServerCache = true;
        string _googleAnalytics = String.Empty;
        string _passThroughToken = String.Empty;

        public bool DebugCss { get { return _debugCss; } set { _debugCss = value; } }
        public bool DebugJs { get { return _debugJs; } set { _debugJs = value; } }
        public bool EnableClientCache { get { return _enableClientCache; } set { _enableClientCache = value; } }
        public bool EnableServerCache { get { return _enableServerCache; } set { _enableServerCache = value; } }
        public string GoogleAnalytics { get { return _googleAnalytics; } set { _googleAnalytics = value; } }
        public string LogProfile { get; set; }
        public LogEventLevel LogMinimumLevel { get; set; }
        public string PassThroughToken { get { return _passThroughToken; } set { _passThroughToken = value; } }
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

            // TODO: validate this? Absolute and well-formed.
            PatternDirectory = nvc.MayGetValue("chiffon:PatternDirectory")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon:PatternDirectory'."));

            LogProfile = nvc.MayGetValue("chiffon:LogProfile")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon:LogProfile'."));

            LogMinimumLevel = nvc.MayParseValue("chiffon:LogMinimumLevel", _ => MayParse.ToEnum<LogEventLevel>(_))
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon:LogMinimumLevel'."));

            // > Paramètres optionels <

            DebugJs = nvc.MayParseValue("chiffon:DebugJs", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(false);

            DebugCss = nvc.MayParseValue("chiffon:DebugCss", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(false);

            EnableClientCache = nvc.MayParseValue("chiffon:EnableClientCache", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(true);

            EnableServerCache = nvc.MayParseValue("chiffon:EnableServerCache", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(true);

            GoogleAnalytics = nvc.MayGetValue("chiffon:GoogleAnalytics").ValueOrElse(String.Empty);

            PassThroughToken = nvc.MayGetValue("chiffon:PassThroughToken").ValueOrElse(String.Empty);
        }
    }
}

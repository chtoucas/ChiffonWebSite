namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Collections;
    using Serilog.Events;

    public class ChiffonConfig
    {
        const string SettingPrefix_ = "chiffon:";
        const string SqlConnectionStringName_ = "SqlServer";

        const bool DefaultDebugCss_ = false;
        const bool DefaultDebugJs_ = false;
        const bool DefaultEnableClientCache_ = true;
        const bool DefaultEnableServerCache_ = true;

        static readonly string DefaultGoogleAnalyticsKey_ = String.Empty;

        static readonly Version AssemblyVersion_
            = Assembly.GetExecutingAssembly().GetName().Version;

        bool _debugCss = DefaultDebugCss_;
        bool _debugJs = DefaultDebugJs_;
        bool _enableClientCache = DefaultEnableClientCache_;
        bool _enableServerCache = DefaultEnableServerCache_;
        string _googleAnalyticsKey = DefaultGoogleAnalyticsKey_;

        public string CssVersion { get; set; }
        public bool DebugCss { get { return _debugCss; } set { _debugCss = value; } }
        public bool DebugJs { get { return _debugJs; } set { _debugJs = value; } }
        public bool EnableClientCache { get { return _enableClientCache; } set { _enableClientCache = value; } }
        public bool EnableServerCache { get { return _enableServerCache; } set { _enableServerCache = value; } }
        public string GoogleAnalyticsKey { get { return _googleAnalyticsKey; } set { _googleAnalyticsKey = value; } }
        public string JsVersion { get; set; }
        public string LogProfile { get; set; }
        public LogEventLevel LogMinimumLevel { get; set; }
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

            LogProfile = nvc.MayGetValue("chiffon:LogProfile")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon:LogProfile'."));

            LogMinimumLevel = nvc.MayParseValue("chiffon:LogMinimumLevel", _ => MayParse.ToEnum<LogEventLevel>(_))
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon:LogMinimumLevel'."));

            // TODO: validate this? Absolute and well-formed.
            PatternDirectory = nvc.MayGetValue("chiffon:PatternDirectory")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon:PatternDirectory'."));

            // > Paramètres optionels <

            var version = String.Format(CultureInfo.InvariantCulture,
                "{0}.{1}.{2}",
                AssemblyVersion_.Major.ToString(CultureInfo.InvariantCulture),
                AssemblyVersion_.Minor.ToString(CultureInfo.InvariantCulture),
                AssemblyVersion_.Build.ToString(CultureInfo.InvariantCulture));

            CssVersion = nvc.MayGetValue("chiffon:CssVersion").ValueOrElse(version);

            JsVersion = nvc.MayGetValue("chiffon:JsVersion").ValueOrElse(version);

            DebugCss = nvc.MayParseValue("chiffon:DebugCss", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(DefaultDebugCss_);

            DebugJs = nvc.MayParseValue("chiffon:DebugJs", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(DefaultDebugJs_);

            EnableClientCache = nvc.MayParseValue("chiffon:EnableClientCache", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(DefaultEnableClientCache_);

            EnableServerCache = nvc.MayParseValue("chiffon:EnableServerCache", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(DefaultEnableServerCache_);

            GoogleAnalyticsKey = nvc.MayGetValue("chiffon:GoogleAnalyticsKey").ValueOrElse(DefaultGoogleAnalyticsKey_);
        }
    }
}

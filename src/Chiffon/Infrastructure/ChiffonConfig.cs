namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Serilog.Events;

    public class ChiffonConfig
    {
        private const string SettingPrefix_ = "chiffon:";
        private const string SqlConnectionStringName_ = "SqlServer";

        private const bool DefaultDebugStyleSheet_ = false;
        private const bool DefaultDebugJavaScript_ = false;
        private const bool DefaultEnableClientCache_ = true;
        private const bool DefaultEnableServerCache_ = true;

        private static readonly string DefaultGoogleAnalyticsKey_ = String.Empty;

        private static readonly Version AssemblyVersion_
            = Assembly.GetExecutingAssembly().GetName().Version;

        private bool _debugStyleSheet = DefaultDebugStyleSheet_;
        private bool _debugJavaScript = DefaultDebugJavaScript_;
        private bool _enableClientCache = DefaultEnableClientCache_;
        private bool _enableServerCache = DefaultEnableServerCache_;
        private string _googleAnalyticsKey = DefaultGoogleAnalyticsKey_;

        public string CssVersion { get; set; }
        public bool DebugStyleSheet { get { return _debugStyleSheet; } set { _debugStyleSheet = value; } }
        public bool DebugJavaScript { get { return _debugJavaScript; } set { _debugJavaScript = value; } }
        public bool EnableClientCache { get { return _enableClientCache; } set { _enableClientCache = value; } }
        public bool EnableServerCache { get { return _enableServerCache; } set { _enableServerCache = value; } }
        public string GoogleAnalyticsKey { get { return _googleAnalyticsKey; } set { _googleAnalyticsKey = value; } }
        public string JavaScriptVersion { get; set; }
        public string LogProfile { get; set; }
        [CLSCompliant(false)]
        public LogEventLevel LogMinimumLevel { get; set; }
        public string PatternDirectory { get; set; }
        public string SqlConnectionString { get; set; }

        public static ChiffonConfig FromConfiguration()
        {
            Contract.Ensures(Contract.Result<ChiffonConfig>() != null);

            return (new ChiffonConfig()).Load();
        }

        public ChiffonConfig Load()
        {
            Contract.Ensures(Contract.Result<ChiffonConfig>() != null);

            LoadSettings_(ConfigurationManager.AppSettings);
            LoadConnectionStrings_(ConfigurationManager.ConnectionStrings);

            return this;
        }

        private void LoadSettings_(NameValueCollection settings)
        {
            Contract.Requires(settings != null);

            var chiffonKeys = settings.AllKeys
                .Where(_ => _.StartsWith(SettingPrefix_, StringComparison.OrdinalIgnoreCase));

            var chiffonSettings = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

            foreach (var key in chiffonKeys)
            {
                chiffonSettings[key] = settings[key];
            }

            Initialize_(chiffonSettings);
        }

        private void LoadConnectionStrings_(ConnectionStringSettingsCollection connections)
        {
            Contract.Requires(connections != null);

            ConnectionStringSettings connection = connections[SqlConnectionStringName_];

            if (connection == null)
            {
                throw new ConfigurationErrorsException(
                    "The '" + SqlConnectionStringName_ + "' connection is not defined in your config file!");
            }

            SqlConnectionString = connection.ConnectionString;
        }

        private void Initialize_(NameValueCollection source)
        {
            Contract.Requires(source != null);

            // > Paramètres obligatoires <

            LogProfile = source.MayGetSingle("chiffon:LogProfile")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon:LogProfile'."));

            LogMinimumLevel = (from _ in source.MayGetSingle("chiffon:LogMinimumLevel")
                               select ParseTo.Enum<LogEventLevel>(_))
                               .UnpackOrThrow(
                                   () => new ConfigurationErrorsException("Missing or invalid config 'chiffon:LogMinimumLevel'."));

            // TODO: validate this? Absolute and well-formed.
            PatternDirectory = source.MayGetSingle("chiffon:PatternDirectory")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon:PatternDirectory'."));

            // > Paramètres optionels <

            var version = AssemblyVersion_.Major.ToString(CultureInfo.InvariantCulture)
                + "." + AssemblyVersion_.Minor.ToString(CultureInfo.InvariantCulture)
                + "." + AssemblyVersion_.Build.ToString(CultureInfo.InvariantCulture);

            CssVersion = source.MayGetSingle("chiffon:CssVersion").ValueOrElse(version);

            JavaScriptVersion = source.MayGetSingle("chiffon:JavaScriptVersion").ValueOrElse(version);

            DebugStyleSheet = (from _ in source.MayGetSingle("chiffon:DebugStyleSheet")
                               select ParseTo.Boolean(_))
                               .UnpackOrElse(DefaultDebugStyleSheet_);

            DebugJavaScript = (from _ in source.MayGetSingle("chiffon:DebugJavaScript")
                               select ParseTo.Boolean(_))
                               .UnpackOrElse(DefaultDebugJavaScript_);

            EnableClientCache = (from _ in source.MayGetSingle("chiffon:EnableClientCache")
                                 select ParseTo.Boolean(_))
                                 .UnpackOrElse(DefaultEnableClientCache_);

            EnableServerCache = (from _ in source.MayGetSingle("chiffon:EnableServerCache")
                                 select ParseTo.Boolean(_))
                                 .UnpackOrElse(DefaultEnableServerCache_);

            GoogleAnalyticsKey = source.MayGetSingle("chiffon:GoogleAnalyticsKey").ValueOrElse(DefaultGoogleAnalyticsKey_);
        }
    }
}

namespace Chiffon.Crosscuttings
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Narvalo;

    public class ChiffonConfig
    {
        const string DebugCssKey_ = "Chiffon/DebugCss";
        const string DebugJsKey_ = "Chiffon/DebugJs";
        const string DisplayNameKey_ = "Chiffon/DisplayName";
        const string HostKey_ = "Chiffon/Host";
        const string PatternDirectoryKey_ = "Chiffon/PatternDirectory";
        const string PortKey_ = "Chiffon/Port";

        bool _debugCss = false;
        bool _debugJs = false;
        string _displayName = "Pour quel motif Simone ?";
        string _host;
        string _patternDirectory;
        int _port = 80;

        ChiffonConfig()
        {
            LoadAndInitialize_(
                ConfigurationManager.AppSettings
                .AllKeys.Select(k => Tuple.Create(k, ConfigurationManager.AppSettings[k])));
        }

        public string PatternDirectory { get { return _patternDirectory; } }
        public bool DebugCss { get { return _debugCss; } }
        public bool DebugJs { get { return _debugJs; } }
        public string DisplayName { get { return _displayName; } }
        public string Host { get { return _host; } }
        public int Port { get { return _port; } }

        public static ChiffonConfig Create()
        {
            return new ChiffonConfig();
        }

        void LoadAndInitialize_(IEnumerable<Tuple<string, string>> values)
        {
            NameValueCollection settings
                = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);

            foreach (var setting in values) {
                if (setting.Item1.StartsWith("Chiffon/", StringComparison.InvariantCultureIgnoreCase)) {
                    settings[setting.Item1] = setting.Item2;
                }
            }

            Initialize_(settings);
        }

        void Initialize_(NameValueCollection settings)
        {
            var debugJs = settings[DebugJsKey_];
            if (!String.IsNullOrEmpty(debugJs)) {
                _debugJs = MayParse.ToBoolean(debugJs, BooleanStyles.Literal)
                    .ValueOrThrow(() => new ConfigurationErrorsException(DebugJsKey_));
            }

            var debugCss = settings[DebugCssKey_];
            if (!String.IsNullOrEmpty(debugCss)) {
                _debugCss = MayParse.ToBoolean(debugCss, BooleanStyles.Literal)
                    .ValueOrThrow(() => new ConfigurationErrorsException(DebugCssKey_));
            }

            var displayName = settings[DisplayNameKey_];
            if (!String.IsNullOrWhiteSpace(displayName)) {
                _displayName = displayName;
            }

            var host = settings[HostKey_];
            if (String.IsNullOrWhiteSpace(host)) {
                throw new ConfigurationErrorsException(HostKey_);
            }
            _host = host;

            var patternDirectory = settings[PatternDirectoryKey_];
            if (String.IsNullOrWhiteSpace(patternDirectory)) {
                throw new ConfigurationErrorsException(PatternDirectoryKey_);
            }
            _patternDirectory = patternDirectory;

            var port = settings[PortKey_];
            if (!String.IsNullOrEmpty(port)) {
                _port = MayParse.ToInt32(port)
                    .ValueOrThrow(() => new ConfigurationErrorsException(PortKey_));
            }
        }
    }
}

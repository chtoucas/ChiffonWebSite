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
        const string BaseUriKey_ = "Chiffon/BaseUri";
        const string DebugCssKey_ = "Chiffon/DebugCss";
        const string DebugJsKey_ = "Chiffon/DebugJs";
        const string DisplayNameKey_ = "Chiffon/DisplayName";
        const string PatternDirectoryKey_ = "Chiffon/PatternDirectory";

        Uri _baseUri;
        bool _debugCss = false;
        bool _debugJs = false;
        string _displayName = "Pour quel motif Simone ?";
        string _patternDirectory;

        ChiffonConfig()
        {
            LoadAndInitialize_(
                ConfigurationManager.AppSettings
                .AllKeys.Select(k => Tuple.Create(k, ConfigurationManager.AppSettings[k])));
        }

        public Uri BaseUri { get { return _baseUri; } }
        public bool DebugCss { get { return _debugCss; } }
        public bool DebugJs { get { return _debugJs; } }
        public string DisplayName { get { return _displayName; } }
        public string PatternDirectory { get { return _patternDirectory; } }

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
            // > Paramètres obligatoires <

            var baseUri = settings[BaseUriKey_];
            if (String.IsNullOrWhiteSpace(baseUri)) {
                throw new ConfigurationErrorsException(BaseUriKey_);
            }
            _baseUri = MayParse.ToUri(baseUri, UriKind.Absolute)
                .ValueOrThrow(() => new ConfigurationErrorsException(BaseUriKey_));


            var patternDirectory = settings[PatternDirectoryKey_];
            if (String.IsNullOrWhiteSpace(patternDirectory)) {
                throw new ConfigurationErrorsException(PatternDirectoryKey_);
            }
            // TODO: validate this, absolute and well-formed.
            _patternDirectory = patternDirectory;

            // > Paramètres optionels <

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
        }
    }
}

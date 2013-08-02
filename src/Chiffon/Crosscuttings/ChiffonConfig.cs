namespace Chiffon.Crosscuttings
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Chiffon.Resources;
    using Narvalo;

    public class ChiffonConfig
    {
        const string BaseUriKey_ = "chiffon.baseUri";
        const string DebugCssKey_ = "chiffon.debugCss";
        const string DebugJsKey_ = "chiffon.debugJs";
        const string DisplayNameKey_ = "chiffon.displayName";
        const string PatternDirectoryKey_ = "chiffon.patternDirectory";

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
                if (setting.Item1.StartsWith("chiffon.", StringComparison.InvariantCultureIgnoreCase)) {
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
                throw NewException_(SR.ChiffonConfig_MissingBaseUri, BaseUriKey_);
            }
            _baseUri = MayParse.ToUri(baseUri, UriKind.Absolute)
                .ValueOrThrow(() => NewException_(SR.ChiffonConfig_InvalidBaseUri, BaseUriKey_));

            var patternDirectory = settings[PatternDirectoryKey_];
            if (String.IsNullOrWhiteSpace(patternDirectory)) {
                throw NewException_(SR.ChiffonConfig_MissingPatternDirectory, PatternDirectoryKey_);
            }
            // TODO: validate this, absolute and well-formed.
            _patternDirectory = patternDirectory;

            // > Paramètres optionels <

            var debugJs = settings[DebugJsKey_];
            if (!String.IsNullOrEmpty(debugJs)) {
                _debugJs = MayParse.ToBoolean(debugJs, BooleanStyles.Literal)
                    .ValueOrThrow(() => NewException_(SR.ChiffonConfig_InvalidDebugJs, DebugJsKey_));
            }

            var debugCss = settings[DebugCssKey_];
            if (!String.IsNullOrEmpty(debugCss)) {
                _debugCss = MayParse.ToBoolean(debugCss, BooleanStyles.Literal)
                    .ValueOrThrow(() => NewException_(SR.ChiffonConfig_InvalidDebugCss, DebugCssKey_));
            }

            var displayName = settings[DisplayNameKey_];
            if (!String.IsNullOrWhiteSpace(displayName)) {
                _displayName = displayName;
            }
        }

        static Exception NewException_(string messageFormat, string key)
        {
            return ExceptionFactory.ConfigurationErrors(messageFormat, key);
        }
    }
}

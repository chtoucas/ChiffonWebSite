namespace Chiffon.Crosscuttings
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Narvalo;
    using Narvalo.Collections;

    public class ChiffonConfig
    {
        ChiffonConfig()
        {
            LoadAndInitialize_(
                ConfigurationManager.AppSettings
                .AllKeys.Select(k => Tuple.Create(k, ConfigurationManager.AppSettings[k])));
        }

        public Uri BaseUri { get; private set; }
        public bool DebugCss { get; private set; }
        public bool DebugJs { get; private set; }
        public string PatternDirectory { get; private set; }

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

        // TODO: cf. PatternImageHandler for a simple way of doing things.
        void Initialize_(NameValueCollection settings)
        {
            // > Paramètres obligatoires <

            BaseUri = settings.MayParseValue("chiffon.baseUri", _ => MayParse.ToUri(_, UriKind.Absolute))
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon.baseUri'."));

            // TODO: validate this, absolute and well-formed.
            PatternDirectory = settings.MayGetValue("chiffon.patternDirectory")
                .ValueOrThrow(() => new ConfigurationErrorsException(
                    "Missing or invalid config 'chiffon.patternDirectory'."));

            // > Paramètres optionels <

            DebugJs = settings.MayParseValue("chiffon.debugJs", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(false);

            DebugCss = settings.MayParseValue("chiffon.debugCss", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                .ValueOrElse(false);
        }
    }
}

namespace Chiffon.Infrastructure.Assets
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using Narvalo;
    using Narvalo.Web.UI.Assets;

    public class ChiffonAssetProvider : AssetProviderBase
    {
        const string BaseUriKey_ = "baseUri";

        Uri _baseUri;

        public ChiffonAssetProvider() : base() { }

        public Uri BaseUri { get { return _baseUri; } }

        public override void Initialize(string name, NameValueCollection config)
        {
            Requires.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "ChiffonAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "XXX");
            }

            base.Initialize(name, config);

            // Initialisation du champs _baseUri.
            string baseUri = config[BaseUriKey_];
            if (String.IsNullOrEmpty(baseUri)) {
                throw new ProviderException("XXX");
            }
            else {
                _baseUri = MayParse.ToUri(baseUri, UriKind.RelativeOrAbsolute)
                    .ValueOrThrow(() => new ProviderException("XXX"));
            }
            config.Remove(BaseUriKey_);

            // FIXME: On vérifie qu'il n'y a pas de champs inconnu restant.
            config.Remove("description");

            if (config.Count > 0) {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr)) {
                    throw new ProviderException("Unrecognized attribute: " + attr);
                }
            }
        }

        public override AssetFile GetImage(string relativePath)
        {
            return new ChiffonImageFile(_baseUri, relativePath);
        }

        public override AssetFile GetScript(string relativePath)
        {
            return new ChiffonScriptFile(_baseUri, relativePath);
        }

        public override AssetFile GetStyle(string relativePath)
        {
            return new ChiffonStyleFile(_baseUri, relativePath);
        }
    }
}

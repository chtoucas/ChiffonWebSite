namespace Chiffon.Infrastructure.Assets
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using Narvalo;
    using Narvalo.Web.UI.Assets;

    public class ChiffonAssetProvider : AssetProviderBase
    {
        const string BaseUrlKey_ = "baseUrl";

        Uri _baseUrl;

        public ChiffonAssetProvider() : base() { }

        public Uri BaseUrl { get { return _baseUrl; } }

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

            // TODO vérifier que les champs sont valides.

            // Initialisation du champs _baseUrl.
            string baseUrlValue = config[BaseUrlKey_];
            if (String.IsNullOrEmpty(baseUrlValue)) {
                throw new ProviderException("XXX");
            }
            else {
                _baseUrl = MayParse.ToUri(baseUrlValue, UriKind.RelativeOrAbsolute)
                    .ValueOrThrow(() => new ProviderException("XXX"));
            }
            config.Remove(BaseUrlKey_);

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
            return new ChiffonImageFile(_baseUrl, relativePath);
        }

        public override AssetFile GetScript(string relativePath)
        {
            return new ChiffonScriptFile(_baseUrl, relativePath);
        }

        public override AssetFile GetStyle(string relativePath)
        {
            return new ChiffonStyleFile(_baseUrl, relativePath);
        }
    }
}

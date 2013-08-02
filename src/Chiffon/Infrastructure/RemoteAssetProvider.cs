namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Web;
    using Narvalo;
    using Narvalo.Web.UI.Assets;

    public class RemoteAssetProvider : AssetProviderBase
    {
        const string BaseUriKey_ = "baseUri";

        Uri _baseUri;

        public RemoteAssetProvider() : base() { }

        public override void Initialize(string name, NameValueCollection config)
        {
            Requires.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "RemoteAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "Chiffon remote asset provider.");
            }

            base.Initialize(name, config);

            // Initialisation du champs baseUri.
            string baseUriValue = config[BaseUriKey_];
            if (String.IsNullOrEmpty(baseUriValue)) {
                throw new ProviderException("XXX");
            }
            _baseUri = MayParse.ToUri(baseUriValue, UriKind.Absolute)
               .ValueOrThrow(() => new ProviderException("XXX"));
            config.Remove(BaseUriKey_);
        }

        public override Uri GetImage(string relativePath)
        {
            return CombineToUri_("/img/", relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            return CombineToUri_("/js/", relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            return CombineToUri_("/css/", relativePath);
        }

        Uri CombineToUri_(string basePath, string relativePath)
        {
            return new Uri(_baseUri, VirtualPathUtility.Combine(basePath, relativePath));
        }
    }
}

namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Web;
    using Chiffon.Resources;
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
                config.Add("description", SR.RemoteAssetProvider_Description);
            }

            base.Initialize(name, config);

            // Initialisation du champs baseUri.
            string baseUriValue = config[BaseUriKey_];
            if (String.IsNullOrEmpty(baseUriValue)) {
                throw new ProviderException(SR.RemoteAssetProvider_BaseUriIsNotAbsolute);
            }
            _baseUri = MayParse.ToUri(baseUriValue, UriKind.Absolute)
               .ValueOrThrow(() => new ProviderException(SR.RemoteAssetProvider_BaseUriIsNotAbsolute));
            config.Remove(BaseUriKey_);
        }

        public override Uri GetImage(string relativePath)
        {
            return MakeUri_("/img/", relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            return MakeUri_("/js/", relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            return MakeUri_("/css/", relativePath);
        }

        Uri MakeUri_(string basePath, string relativePath)
        {
            return new Uri(_baseUri, VirtualPathUtility.Combine(basePath, relativePath));
        }
    }
}

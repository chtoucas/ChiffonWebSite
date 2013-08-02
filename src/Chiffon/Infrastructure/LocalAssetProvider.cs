namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using Narvalo;
    using Narvalo.Web.UI.Assets;

    public class LocalAssetProvider : AssetProviderBase
    {
        public LocalAssetProvider() : base() { }

        public override void Initialize(string name, NameValueCollection config)
        {
            Requires.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "LocalAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "Chiffon local asset provider.");
            }

            base.Initialize(name, config);
        }

        public override Uri GetImage(string relativePath)
        {
            return CombineToUri_("~/assets/img/", relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            return CombineToUri_("~/assets/js/", relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            return CombineToUri_("~/assets/css/", relativePath);
        }

        static Uri CombineToUri_(string basePath, string relativePath)
        {
            return new Uri(
                VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(basePath, relativePath)),
                UriKind.Relative);
        }
    }
}

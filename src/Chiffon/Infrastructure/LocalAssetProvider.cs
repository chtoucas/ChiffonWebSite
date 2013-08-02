namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using Chiffon.Resources;
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
                config.Add("description", SR.LocalAssetProvider_Description);
            }

            base.Initialize(name, config);
        }

        public override Uri GetImage(string relativePath)
        {
            return MakeUri_("~/assets/img/", relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            return MakeUri_("~/assets/js/", relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            return MakeUri_("~/assets/css/", relativePath);
        }

        static Uri MakeUri_(string basePath, string relativePath)
        {
            return new Uri(Combine_(basePath, relativePath), UriKind.Relative);
        }

        static string Combine_(string basePath, string relativePath)
        {
            return VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(basePath, relativePath));
        }
    }
}

namespace Chiffon.Common
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo;
    using Narvalo.Web.UI.Assets;

    public sealed class LocalAssetProvider : AssetProviderBase
    {
        public LocalAssetProvider() { }

        public override void Initialize(string name, NameValueCollection config)
        {
            Require.NotNull(config, "config");

            if (String.IsNullOrEmpty(name))
            {
                name = "LocalAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Chiffon local asset provider.");
            }

            base.Initialize(name, config);
        }

        public override Uri GetFont(string relativePath)
        {
            return MakeUri_("~/assets/font/", relativePath);
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

        private static Uri MakeUri_(string basePath, string relativePath)
        {
            Contract.Requires(relativePath != null);

            return new Uri(Combine_(basePath, relativePath), UriKind.Relative);
        }

        private static string Combine_(string basePath, string relativePath)
        {
            Contract.Requires(relativePath != null);

            return VirtualPathUtility.ToAbsolute(
                relativePath.Length == 0 ? basePath : VirtualPathUtility.Combine(basePath, relativePath));
        }
    }
}

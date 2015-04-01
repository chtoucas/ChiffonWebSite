namespace Chiffon.Common
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Web;

    using Chiffon.Entities;

    //<configuration>
    //  <uri>
    //  <idn enabled="All" />
    //  <iriParsing enabled="true" />
    //  </uri>
    //</configuration>
    // Check UriBuilder
    // TODO: ajouter la possibilité de supprimer le préfixe http:
    // - change base path, depending on anonymous / member
    // http://stackoverflow.com/questions/829080/how-to-build-a-query-string-for-a-url-in-c
    // VirtualPathUtility
    // NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
    // Cf. http://stackoverflow.com/questions/3797182/how-to-correctly-canonicalize-a-url-in-an-asp-net-mvc-application
    // & https://github.com/schourode/canonicalize
    public sealed class SingleDomainSiteMap : ISiteMap
    {
        private readonly Uri _baseUri;
        private readonly ChiffonLanguage _language;

        public SingleDomainSiteMap(ChiffonEnvironment environment)
        {
            if (!environment.BaseUri.IsAbsoluteUri)
            {
                throw new ArgumentException("The 'baseUri' parameter must be absolute.", "environment");
            }

            _baseUri = environment.BaseUri;
            _language = environment.Language;
        }

        public ChiffonLanguage Language { get { return _language; } }

        public Uri Home() { return MakeAbsoluteUri_(String.Empty); }

        public Uri About() { return MakeAbsoluteUri_(Constants.RoutePath.About); }

        public Uri Contact() { return MakeAbsoluteUri_(Constants.RoutePath.Contact); }

        public Uri Newsletter() { return MakeAbsoluteUri_(Constants.RoutePath.Newsletter); }

        public Uri Login() { return MakeAbsoluteUri_(Constants.RoutePath.Login); }

        public Uri Login(Uri returnUrl)
        {
            var uri = MakeAbsoluteUri_(Constants.RoutePath.Login);
            return AddReturnUrl_(uri, returnUrl);
        }

        public Uri Register() { return MakeAbsoluteUri_(Constants.RoutePath.Register); }

        public Uri Register(Uri returnUrl)
        {
            var uri = MakeAbsoluteUri_(Constants.RoutePath.Register);
            return AddReturnUrl_(uri, returnUrl);
        }

        public Uri Designer(DesignerKey designerKey, int pageIndex)
        {
            var uri = MakeAbsoluteUri_(designerKey.ToString());
            return AddPagination_(uri, pageIndex);
        }

        public Uri DesignerCategory(DesignerKey designerKey, string categoryKey, int pageIndex)
        {
            var uri = MakeAbsoluteUri_(designerKey.ToString() + "/" + categoryKey);
            return AddPagination_(uri, pageIndex);
        }

        public Uri DesignerPattern(DesignerKey designerKey, string categoryKey, string reference, int pageIndex)
        {
            var uri = MakeAbsoluteUri_(designerKey.ToString() + "/" + categoryKey + "/" + reference);
            return AddPagination_(uri, pageIndex);
        }

        private static Uri AddPagination_(Uri uri, int pageIndex)
        {
            Contract.Requires(uri != null);

            if (pageIndex > 1)
            {
                var builder = new UriBuilder(uri) {
                    Query = Constants.SiteMap.PageKey + "=" + pageIndex.ToString(CultureInfo.InvariantCulture)
                };

                return builder.Uri;
            }
            else
            {
                return uri;
            }
        }

        // NB: "returnUrl" contient le répertoire virtuel.
        private static Uri AddReturnUrl_(Uri uri, Uri returnUrl)
        {
            Contract.Requires(uri != null);
            Contract.Requires(returnUrl != null);

            var builder = new UriBuilder(uri) {
                Query = Constants.SiteMap.ReturnUrl + "=" + returnUrl.ToString()
            };

            return builder.Uri;
        }

        // NB: "relativePath" ne contient pas le répertoire virtuel.
        private Uri MakeAbsoluteUri_(string relativePath)
        {
            Contract.Requires(relativePath != null);

            if (relativePath.Length == 0)
            {
                return new Uri(_baseUri, VirtualPathUtility.ToAbsolute("~/"));
            }
            else
            {
                return new Uri(_baseUri, VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine("~/", relativePath)));
            }
        }
    }
}

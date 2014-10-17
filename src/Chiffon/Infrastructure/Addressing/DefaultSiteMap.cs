namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Globalization;
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
    public class DefaultSiteMap : ISiteMap
    {
        readonly Uri _baseUri;
        readonly ChiffonLanguage _language;

        public DefaultSiteMap(ChiffonEnvironment environment)
        {
            if (!environment.BaseUri.IsAbsoluteUri) {
                throw new ArgumentException("The 'baseUri' parameter must be absolute.", "environment");
            }

            _baseUri = environment.BaseUri;
            _language = environment.Language;
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class Constants
        {
            public const string About = "informations";
            public const string Contact = "contact";
            public const string Newsletter = "newsletter";
            [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
            public const string Login = "connexion";
            public const string Register = "inscription";

            // TODO: Utiliser ces constantes dans SiteMap (cf. SmartFormat.NET).
            public const string Designer = "{designerKey}";
            public const string DesignerCategory = "{designerKey}/{categoryKey}";
            public const string DesignerPattern = "{designerKey}/{categoryKey}/{reference}";

            public const string LogOff = "disconnecte";
            public const string LogOn = "connecte";
            public const string Pattern = "motif";
        }

        #region ISiteMap

        public ChiffonLanguage Language { get { return _language; } }

        public Uri BaseUri { get { return _baseUri; } }

        //public Uri Home() { return _baseUri; }
        public Uri Home() { return MakeAbsoluteUri_(String.Empty); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri About() { return MakeAbsoluteUri_(Constants.About); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Contact() { return MakeAbsoluteUri_(Constants.Contact); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Newsletter() { return MakeAbsoluteUri_(Constants.Newsletter); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public Uri Login() { return MakeAbsoluteUri_(Constants.Login); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public Uri Login(Uri returnUrl)
        {
            var uri = MakeAbsoluteUri_(Constants.Login);
            return AddReturnUrl_(uri, returnUrl);
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Register() { return MakeAbsoluteUri_(Constants.Register); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Register(Uri returnUrl)
        {
            var uri = MakeAbsoluteUri_(Constants.Register);
            return AddReturnUrl_(uri, returnUrl);
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Designer(DesignerKey designerKey, int pageIndex)
        {
            var uri = MakeAbsoluteUri_(designerKey.ToString());
            return AddPagination_(uri, pageIndex);
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri DesignerCategory(DesignerKey designerKey, string categoryKey, int pageIndex)
        {
            var uri = MakeAbsoluteUri_(designerKey.ToString() + "/" + categoryKey);
            return AddPagination_(uri, pageIndex);
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri DesignerPattern(DesignerKey designerKey, string categoryKey, string reference, int pageIndex)
        {
            var uri = MakeAbsoluteUri_(designerKey.ToString() + "/" + categoryKey + "/" + reference);
            return AddPagination_(uri, pageIndex);
        }

        #endregion

        static Uri AddPagination_(Uri uri, int pageIndex)
        {
            if (pageIndex > 1) {
                var builder = new UriBuilder(uri) {
                    Query = String.Format(CultureInfo.InvariantCulture,
                        "{0}={1}", SiteMapConstants.PageKey, pageIndex.ToString(CultureInfo.InvariantCulture))
                };
                return builder.Uri;
            }
            else {
                return uri;
            }
        }

        // returnUrl contient le répertoire virtuel.
        static Uri AddReturnUrl_(Uri uri, Uri returnUrl)
        {
            var builder = new UriBuilder(uri) {
                Query = String.Format(CultureInfo.InvariantCulture,
                    "{0}={1}", SiteMapConstants.ReturnUrl, returnUrl.ToString())
            };
            return builder.Uri;
        }

        // relativeUri ne contient pas le répertoire virtuel.
        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings"), SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
        Uri MakeAbsoluteUri_(string relativeUri)
        {
            return new Uri(_baseUri, VirtualPathUtility.ToAbsolute("~/" + relativeUri));
        }
    }
}

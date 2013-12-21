namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net;
    using Chiffon.Entities;
    using Narvalo;

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
                throw new ArgumentException("The 'baseUri' parameter must be absolute.");
            }

            _baseUri = environment.BaseUri;
            _language = environment.Language;
        }

        #region ISiteMap

        public ChiffonLanguage Language { get { return _language; } }

        public Uri BaseUri { get { return _baseUri; } }

        public Uri Home() { return _baseUri; }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri About() { return MakeAbsoluteUri("informations"); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Contact() { return MakeAbsoluteUri("contact"); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri ContactSuccess() { return MakeAbsoluteUri("contact-confirmation"); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Newsletter() { return MakeAbsoluteUri("newsletter"); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri LogOn() { return MakeAbsoluteUri("connexion"); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri LogOn(Uri targetUrl)
        {
            Requires.NotNull(targetUrl, "targetUrl");

            if (targetUrl.IsAbsoluteUri) {
                throw new ArgumentException("XXX", "targetUrl");
            }

            var builder = new UriBuilder(MakeAbsoluteUri("connexion")) {
                //Query = "targetUrl=XXX",
            };
            return builder.Uri;
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Login() { return MakeAbsoluteUri("connexion"); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Register() { return MakeAbsoluteUri("inscription"); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri RegisterSuccess() { return MakeAbsoluteUri("inscription-confirmation"); }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri Designer(DesignerKey designerKey, int pageIndex)
        {
            var uri = MakeAbsoluteUri(designerKey.ToString() + "/");
            return AddPagination_(uri, pageIndex);
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri DesignerCategory(DesignerKey designerKey, string categoryKey, int pageIndex)
        {
            var uri = MakeAbsoluteUri(designerKey.ToString() + "/" + categoryKey);
            return AddPagination_(uri, pageIndex);
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri DesignerPattern(DesignerKey designerKey, string categoryKey, string reference, int pageIndex)
        {
            var uri = MakeAbsoluteUri(designerKey.ToString() + "/" + categoryKey + "/" + reference);
            return AddPagination_(uri, pageIndex);
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings"), SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
        public Uri MakeAbsoluteUri(string relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }

        public Uri MakeAbsoluteUri(Uri relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }

        #endregion

        static Uri AddPagination_(Uri uri, int pageIndex)
        {
            if (pageIndex > 1) {
                var builder = new UriBuilder(uri) {
                    Query = "p=" + pageIndex.ToString(CultureInfo.InvariantCulture)
                };
                return builder.Uri;
            }
            else {
                return uri;
            }
        }
    }
}

namespace Chiffon.Infrastructure.Addressing
{
    using System;
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
    // - change base Uri => absolute / relative
    // - remove the http: part
    // - https
    // - I18N
    // http://stackoverflow.com/questions/829080/how-to-build-a-query-string-for-a-url-in-c
    // VirtualPathUtility
    // NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
    public class DefaultSiteMap : ISiteMap
    {
        private readonly Uri _baseUri;

        internal DefaultSiteMap(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri Home()
        {
            return _baseUri;
        }

        public Uri AboutUs()
        {
            return new Uri(_baseUri, "à-propos");
        }

        public Uri LogOn()
        {
            return new Uri(_baseUri, "connexion");
        }

        public Uri LogOn(LogOnOptions options)
        {
            Requires.NotNull(options, "options");

            UriBuilder builder = new UriBuilder(new Uri(_baseUri, "connexion")) {
                Query = options.ToQueryString()
            };
            return builder.Uri;
        }

        public Uri MakeAbsoluteUri(Uri targetUrl)
        {
            return new Uri(_baseUri, targetUrl);
        }
    }
}

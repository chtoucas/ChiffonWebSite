namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using Chiffon.Crosscuttings;
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
    public class SiteMap : ISiteMap
    {
        readonly Uri _baseUri;

        public SiteMap(ChiffonConfig config) : this(config.BaseUri) { }

        public SiteMap(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri LogOn()
        {
            return new Uri(_baseUri, "connexion");
        }

        public Uri LogOn(Uri targetUrl)
        {
            // TODO: Valider targetUrl.IsRelative.
            UriBuilder builder = new UriBuilder(new Uri(_baseUri, "connexion")) {
                Query = "targetUrl=XXX",
            };
            return builder.Uri;
        }

        public Uri MakeAbsoluteUri(Uri targetUrl)
        {
            return new Uri(_baseUri, targetUrl);
        }
    }
}

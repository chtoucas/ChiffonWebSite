namespace Chiffon.Infrastructure.Addressing
{
    using System;
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

        public DefaultSiteMap(Uri baseUri)
        {
            Requires.NotNull(baseUri, "baseUri");

            if (!baseUri.IsAbsoluteUri) {
                throw new ArgumentException("The 'baseUri' parameter must be absolute.");
            }

            _baseUri = baseUri;
        }

        #region ISiteMap

        public Uri BaseUri { get { return _baseUri; } }

        public Uri Home() { return _baseUri; }
        public Uri About() { return MakeAbsoluteUri("informations"); }
        public Uri Contact() { return MakeAbsoluteUri("contact"); }
        public Uri Newsletter() { return MakeAbsoluteUri("newsletter"); }

        public Uri LogOn() { return MakeAbsoluteUri("connexion"); }

        public Uri LogOn(Uri targetUrl)
        {
            if (targetUrl.IsAbsoluteUri) {
                throw new ArgumentException("XXX", "targetUrl");
            }

            UriBuilder builder = new UriBuilder(MakeAbsoluteUri("connexion")) {
                Query = "targetUrl=XXX",
            };
            return builder.Uri;
        }

        public Uri Login() { return MakeAbsoluteUri("connexion"); }
        public Uri Register() { return MakeAbsoluteUri("informations"); }

        public Uri Designer(DesignerKey designer)
        {
            return MakeAbsoluteUri(designer.ToString() + "/");
        }

        public Uri DesignerCategory(DesignerKey designer, string category)
        {
            return MakeAbsoluteUri(designer.ToString() + "/" + category);
        }

        public Uri DesignerPattern(DesignerKey designer, string reference)
        {
            return MakeAbsoluteUri(designer.ToString() + "/" + reference);
        }

        public Uri MakeAbsoluteUri(string relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }

        public Uri MakeAbsoluteUri(Uri relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }

        #endregion
    }
}

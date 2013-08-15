namespace Chiffon.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using Narvalo;

    public class ChiffonEnvironment
    {
        static object Lock_ = new Object();
        static ChiffonEnvironment Current_;

        readonly Uri _baseUri;
        readonly ChiffonCulture _culture;

        public ChiffonEnvironment(ChiffonCulture culture, Uri baseUri)
        {
            Requires.NotNull(culture, "culture");
            Requires.NotNull(baseUri, "baseUri");

            _baseUri = baseUri;
            _culture = culture;
        }

        public Uri BaseUri { get { return _baseUri; } }

        public ChiffonCulture Culture { get { return _culture; } }

        public static ChiffonEnvironment Current
        {
            get { return Current_; }
            private set { lock (Lock_) { Current_ = value; } }
        }

        public static ChiffonEnvironment ResolveAndInitialize(ChiffonConfig config, HttpRequest request)
        {
            ChiffonCulture culture;
            string domainName;

            if (request.Url.Host == config.DomainName) {
                domainName = config.DomainName;
                culture = ChiffonCulture.Create(ChiffonLanguage.Default);
            }
            else {
                culture = ChiffonCulture.Create(ChiffonLanguage.English);
                domainName = String.Format(CultureInfo.InvariantCulture,
                    "{0}.{1}", culture.Language, config.DomainName);

                InitializeCulture_(culture);
            }

            var builder = new UriBuilder {
                Scheme = Uri.UriSchemeHttp,
                Host = domainName,
            };

            Current = new ChiffonEnvironment(culture, builder.Uri);

            return Current;
        }

        static void InitializeCulture_(ChiffonCulture culture)
        {
            // Culture utilisée par ResourceManager.
            Thread.CurrentThread.CurrentUICulture = culture.UICulture;
            // Culture utilisée par System.Globalization.
            Thread.CurrentThread.CurrentCulture = culture.Culture;
        }

        //static Uri GetBaseUri_(HttpRequestBase request)
        //{
        //    return new Uri(request.Url.GetLeftPart(UriPartial.Authority), UriKind.Absolute);

        //    // return VirtualPathUtility.ToAbsolute("~/");
        //    // return new Uri(request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
        //    // return new Uri(request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped));
        //}
    }
}
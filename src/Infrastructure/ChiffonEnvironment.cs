namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web;
    using Narvalo;
    using Narvalo.Collections;

    public class ChiffonEnvironment
    {
        static readonly Dictionary<String, ChiffonLanguage> DomainLanguage_
            = new Dictionary<string, ChiffonLanguage>() {
                {"pourquelmotifsimone.com",    ChiffonLanguage.Default},
                {"en.pourquelmotifsimone.com", ChiffonLanguage.English},
            };

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

        public static ChiffonEnvironment ResolveAndInitialize(HttpRequest request)
        {
            var info = ResolveDomain_(request);
            var culture = ChiffonCulture.Create(info.Language);

            if (info.Language != ChiffonLanguage.Default) {
                InitializeCulture_(culture);
            }

            return (Current = new ChiffonEnvironment(culture, info.Uri));
        }

        class DomainInfo
        {
            public ChiffonLanguage Language { get; set; }
            public Uri Uri { get; set; }
        }

        static Uri GetBaseUri_(Uri uri)
        {
            return new Uri(uri.GetLeftPart(UriPartial.Authority), UriKind.Absolute);

            // return VirtualPathUtility.ToAbsolute("~/") -> "/";
            // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
            // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
        }

        static void InitializeCulture_(ChiffonCulture culture)
        {
            // Culture utilisée par ResourceManager.
            Thread.CurrentThread.CurrentUICulture = culture.UICulture;
            // Culture utilisée par System.Globalization.
            Thread.CurrentThread.CurrentCulture = culture.Culture;
        }

        static DomainInfo ResolveDomain_(HttpRequest request)
        {
            var uri = GetBaseUri_(request.Url);
            var language = uri.IsLoopback
                ? ResolveLanguageFromQueryString_(request)
                : ResolveLanguageFromHost_(uri);

            return new DomainInfo {
                Language = language,
                Uri = uri,
            };
        }

        static ChiffonLanguage ResolveLanguageFromHost_(Uri uri)
        {
            return DomainLanguage_.MayGetValue(uri.Host)
                .ValueOrThrow(() => new NotSupportedException());
        }

        static ChiffonLanguage ResolveLanguageFromQueryString_(HttpRequest request)
        {
            return request.QueryString
                .MayParseValue("lang", _ => MayParse.ToEnum<ChiffonLanguage>(_))
                .ValueOrElse(ChiffonLanguage.Default);
        }
    }
}
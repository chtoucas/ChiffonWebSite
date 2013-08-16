namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Narvalo;
    using Narvalo.Collections;

    public static class ChiffonEnvironmentResolver
    {
        public static IEnumerable<ChiffonEnvironment> Environments
        {
            get
            {
                yield return new ChiffonEnvironment(ChiffonLanguage.Default, new Uri("http://pourquelmotifsimone.com"));
                yield return new ChiffonEnvironment(ChiffonLanguage.English, new Uri("http://en.pourquelmotifsimone.com"));
            }
        }

        static Uri GetBaseUri_(Uri uri)
        {
            return new Uri(uri.GetLeftPart(UriPartial.Authority), UriKind.Absolute);

            // return VirtualPathUtility.ToAbsolute("~/") -> "/";
            // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
            // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
        }

        public static ChiffonEnvironment Resolve(HttpRequest request)
        {
            var uri = GetBaseUri_(request.Url);

            ChiffonEnvironment environment;
            if (uri.IsLoopback) {
                var language = ResolveLanguageFromQueryString_(request);
                environment = new ChiffonEnvironment(language, uri);
            }
            else {
                environment = ResolveFromHost_(uri);
            }

            return environment;
        }

        static ChiffonEnvironment ResolveFromHost_(Uri uri)
        {
            var q = from _ in Environments where _.BaseUri.Host == uri.Host select _;

            return q.SingleOrNone().ValueOrThrow(() => new NotSupportedException());
        }

        static ChiffonLanguage ResolveLanguageFromQueryString_(HttpRequest request)
        {
            return request.QueryString
                .MayParseValue("lang", _ => MayParse.ToEnum<ChiffonLanguage>(_))
                .ValueOrElse(ChiffonLanguage.Default);
        }
    }

}
namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.SessionState;

    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Fx.Extensions;

    public static class ChiffonEnvironmentResolver
    {
        const string SessionKey_ = "Language";

#if SHOWCASE
        static readonly ChiffonEnvironment DefaultEnvironment_
            = new ChiffonEnvironment(ChiffonLanguage.Default, new Uri("http://narvalo.org"), ChiffonHosting.SingleDomain);
        static readonly ChiffonEnvironment EnglishEnvironment_
            = new ChiffonEnvironment(ChiffonLanguage.English, new Uri("http://narvalo.org"), ChiffonHosting.SingleDomain);
#else
        static readonly ChiffonEnvironment DefaultEnvironment_
            = new ChiffonEnvironment(ChiffonLanguage.Default, new Uri("http://pourquelmotifsimone.com"), ChiffonHosting.OneDomainPerLanguage);
        static readonly ChiffonEnvironment EnglishEnvironment_
            = new ChiffonEnvironment(ChiffonLanguage.English, new Uri("http://en.pourquelmotifsimone.com"), ChiffonHosting.OneDomainPerLanguage);
#endif

        // FIXME: Ne marche pas dans le cas où on utilise un environnement différent 
        // de ce qui est prévu à l'origine (cf. Resolve()). Il faudra initialiser
        // cette propriété dynamiquement.
        public static IEnumerable<ChiffonEnvironment> Environments
        {
            get
            {
                yield return DefaultEnvironment_;
                yield return EnglishEnvironment_;
            }
        }

        public static ChiffonEnvironment DefaultEnvironment
        {
            get { return DefaultEnvironment_; }
        }

        public static ChiffonEnvironment Resolve(HttpRequest request)
        {
            Require.NotNull(request, "request");

            var uri = GetBaseUri_(request.Url);
            return ResolveFromHost_(uri.Host);
        }

        internal static ChiffonEnvironment Resolve(HttpRequest request, HttpSessionState session)
        {
            Require.NotNull(request, "request");

            // FIXME: Ne marche pas quand on place l'application dans un répertoire virtuel.
            var uri = GetBaseUri_(request.Url);

            var language = GetLanguageFromQueryString_(request);

            // Si le visiteur a demandé une langue bien spécifique, on sauvegarde la demande en session.
            language.OnValue(_ => { UpdateLanguageSession_(session, _); });

            // On regarde dans la session si on n'a pas une langue déjà définie.
            language = language ?? GetLanguageFromSession_(session);

            // On détermine comment le passage d'une langue à une autre va se faire.
            bool isAppVirtual = VirtualPathUtility.ToAbsolute("~/") == "/";
            var hosting = isAppVirtual ? ChiffonHosting.SingleDomain : ChiffonHosting.OneDomainPerLanguage;

            return language.Select(_ => new ChiffonEnvironment(_, uri, hosting))
                ?? new ChiffonEnvironment(ChiffonLanguage.Default, uri, hosting);
        }

        static Uri GetBaseUri_(Uri uri)
        {
            return new Uri(uri.GetLeftPart(UriPartial.Authority), UriKind.Absolute);
        }

        //static Uri GetBaseUri_(HttpRequest request)
        //{
        //    // http://msdn.microsoft.com/en-us/library/system.web.httpruntime.appdomainappvirtualpath(v=vs.110).aspx
        //    // http://weblog.west-wind.com/posts/2009/Dec/21/Making-Sense-of-ASPNET-Paths

        //    string authority = request.Url.GetLeftPart(UriPartial.Authority);
        //    var uriBuilder = new UriBuilder(authority);
        //    uriBuilder.Path = request.ApplicationPath;
        //    return uriBuilder.Uri;

        //    // return VirtualPathUtility.ToAbsolute("~/") -> "/";
        //    // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
        //    // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
        //}

        static ChiffonLanguage? GetLanguageFromQueryString_(HttpRequest request)
        {
            return (from _ in request.QueryString.MayGetSingle("lang")
                    select ParseTo.Enum<ChiffonLanguage>(_)).ToNullable();
        }

        static ChiffonLanguage? GetLanguageFromSession_(HttpSessionState session)
        {
            var value = session[SessionKey_];

            return value != null ? ConvertTo.Enum<ChiffonLanguage>(value) : null;
        }

        static ChiffonEnvironment ResolveFromHost_(string host)
        {
            var q = from _ in Environments where _.BaseUri.Host == host select _;

            return q.SingleOrNone().ValueOrElse(DefaultEnvironment);
        }

        static void UpdateLanguageSession_(HttpSessionState session, ChiffonLanguage language)
        {
            session[SessionKey_] = language;
        }
    }
}
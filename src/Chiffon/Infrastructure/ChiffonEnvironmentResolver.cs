namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using System.Web.SessionState;

    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Fx.Extensions;

    public static class ChiffonEnvironmentResolver
    {
        private const string SESSION_KEY = "Language";

#if SHOWCASE
        private static readonly ChiffonEnvironment s_DefaultEnvironment
              = new ChiffonEnvironment(ChiffonLanguage.Default, new Uri("http://vivianedevaux.org"), ChiffonHosting.SingleDomain);

        private static readonly ChiffonEnvironment s_EnglishEnvironment
              = new ChiffonEnvironment(ChiffonLanguage.English, new Uri("http://vivianedevaux.org"), ChiffonHosting.SingleDomain);
#else
     private   static readonly ChiffonEnvironment s_DefaultEnvironment
            = new ChiffonEnvironment(ChiffonLanguage.Default, new Uri("http://pourquelmotifsimone.com"), ChiffonHosting.OneDomainPerLanguage);
  
     private   static readonly ChiffonEnvironment s_EnglishEnvironment
            = new ChiffonEnvironment(ChiffonLanguage.English, new Uri("http://en.pourquelmotifsimone.com"), ChiffonHosting.OneDomainPerLanguage);
#endif

        // FIXME: Ne marche pas dans le cas où on utilise un environnement différent 
        // de ce qui est prévu à l'origine (cf. Resolve()). Il faudra initialiser
        // cette propriété dynamiquement.
        public static IEnumerable<ChiffonEnvironment> Environments
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<ChiffonEnvironment>>() != null);

                yield return s_DefaultEnvironment;
                yield return s_EnglishEnvironment;
            }
        }

        public static ChiffonEnvironment DefaultEnvironment
        {
            get { return s_DefaultEnvironment; }
        }

        public static ChiffonEnvironment Resolve(HttpRequest request)
        {
            Require.NotNull(request, "request");

            var uri = GetBaseUri_(request.Url);
            return ResolveFromHost_(uri.Host);
        }

        public static ChiffonEnvironment Resolve(HttpRequest request, HttpSessionState session)
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

        private static Uri GetBaseUri_(Uri uri)
        {
            Contract.Requires(uri != null);

            return new Uri(uri.GetLeftPart(UriPartial.Authority), UriKind.Absolute);
        }

        //private static Uri GetBaseUri_(HttpRequest request)
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

        private static ChiffonLanguage? GetLanguageFromQueryString_(HttpRequest request)
        {
            Contract.Requires(request != null);

            return (from _ in request.QueryString.MayGetSingle("lang")
                    select ParseTo.Enum<ChiffonLanguage>(_)).ToNullable();
        }

        private static ChiffonLanguage? GetLanguageFromSession_(HttpSessionState session)
        {
            Contract.Requires(session != null);

            var value = session[SESSION_KEY];

            if (value == null)
            {
                return null;
            }

            var type = typeof(ChiffonLanguage);

            if (Enum.IsDefined(type, value))
            {
                return (ChiffonLanguage)Enum.ToObject(type, value);
            }
            else
            {
                return null;
            }
        }

        private static ChiffonEnvironment ResolveFromHost_(string host)
        {
            var q = from _ in Environments where _.BaseUri.Host == host select _;

            return q.SingleOrNone().ValueOrElse(DefaultEnvironment);
        }

        private static void UpdateLanguageSession_(HttpSessionState session, ChiffonLanguage language)
        {
            Contract.Requires(session != null);

            session[SESSION_KEY] = language;
        }
    }
}
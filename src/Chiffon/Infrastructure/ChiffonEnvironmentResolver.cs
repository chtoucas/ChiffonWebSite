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
    using Narvalo.Linq;

    public static class ChiffonEnvironmentResolver
    {
        const string SessionKey_ = "Language";

        static readonly ChiffonEnvironment DefaultEnvironment_
            = new ChiffonEnvironment(ChiffonLanguage.Default, new Uri("http://pourquelmotifsimone.com"));
        static readonly ChiffonEnvironment EnglishEnvironment_
            = new ChiffonEnvironment(ChiffonLanguage.English, new Uri("http://en.pourquelmotifsimone.com"));

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

            var uri = GetBaseUri_(request.Url);

            var language = GetLanguageFromQueryString_(request);

            // Si le visiteur a demandé une langue bien spécifique, on sauvegarde la demande en session.
            language.OnValue(_ => { UpdateLanguageSession_(session, _); });

            // On regarde dans la session si on n'a pas une langue déjà définie.
            language = language ?? GetLanguageFromSession_(session);

            return language.Map(_ => new ChiffonEnvironment(_, uri))
                ?? new ChiffonEnvironment(ChiffonLanguage.Default, uri);
        }

        static Uri GetBaseUri_(Uri uri)
        {
            return new Uri(uri.GetLeftPart(UriPartial.Authority), UriKind.Absolute);

            // return VirtualPathUtility.ToAbsolute("~/") -> "/";
            // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
            // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
        }

        static ChiffonLanguage? GetLanguageFromQueryString_(HttpRequest request)
        {
            return (from _ in request.QueryString.MayGetSingle("lang")
                    select ParseTo.NullableEnum<ChiffonLanguage>(_)).ToNullable();
        }

        static ChiffonLanguage? GetLanguageFromSession_(HttpSessionState session)
        {
            var value = session[SessionKey_];
            ChiffonLanguage result;

            if (TryConvertTo.Enum<ChiffonLanguage>(value, out result)) {
                return result;
            }
            else {
                return null;
            }
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
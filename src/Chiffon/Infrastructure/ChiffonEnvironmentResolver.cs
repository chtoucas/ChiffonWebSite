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

    public static class ChiffonEnvironmentResolver
    {
        static string SessionKey_ = "Language";
        static ChiffonEnvironment DefaultEnvironment_
            = new ChiffonEnvironment(ChiffonLanguage.Default, new Uri("http://pourquelmotifsimone.com"));

        public static IEnumerable<ChiffonEnvironment> Environments
        {
            get
            {
                yield return DefaultEnvironment_;
                yield return new ChiffonEnvironment(ChiffonLanguage.English, new Uri("http://en.pourquelmotifsimone.com"));
            }
        }

        public static ChiffonEnvironment Resolve(HttpRequest request)
        {
            Requires.NotNull(request, "request");

            var uri = GetBaseUri_(request.Url);
            return ResolveFromHost_(uri.Host);
        }

        internal static ChiffonEnvironment Resolve(HttpRequest request, HttpSessionState session)
        {
            Requires.NotNull(request, "request");

            var uri = GetBaseUri_(request.Url);

            if (!uri.IsLoopback) {
                throw new InvalidOperationException(
                    "This method is only available when run locally.");
            }

            var language = MayGetLanguageFromQueryString_(request);

            // Si le visiteur a demandé une langue bien spécifique, on sauvegarde
            // la demande en session.
            language.WhenSome(_ => { UpdateLanguageSession_(session, _); });

            if (language.IsNone) {
                // On regarde dans la session si on n'a pas une langue déjà définie.
                language = MayGetLanguageFromSession_(session);
            }

            return new ChiffonEnvironment(language.ValueOrElse(ChiffonLanguage.Default), uri);
        }

        static Uri GetBaseUri_(Uri uri)
        {
            return new Uri(uri.GetLeftPart(UriPartial.Authority), UriKind.Absolute);

            // return VirtualPathUtility.ToAbsolute("~/") -> "/";
            // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
            // return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
        }

        static Maybe<ChiffonLanguage> MayGetLanguageFromQueryString_(HttpRequest request)
        {
            return request.QueryString
                .MayParseValue("lang", _ => MayParse.ToEnum<ChiffonLanguage>(_));
        }

        static Maybe<ChiffonLanguage> MayGetLanguageFromSession_(HttpSessionState session)
        {
            var value = session[SessionKey_];

            return value == null
                ? Maybe<ChiffonLanguage>.None
                : EnumUtility.MayConvert<ChiffonLanguage>(value);
        }

        static ChiffonEnvironment ResolveFromHost_(string host)
        {
            var q = from _ in Environments where _.BaseUri.Host == host select _;

            return q.SingleOrNone().ValueOrElse(DefaultEnvironment_);
        }

        static void UpdateLanguageSession_(HttpSessionState session, ChiffonLanguage language)
        {
            session[SessionKey_] = language;
        }
    }
}
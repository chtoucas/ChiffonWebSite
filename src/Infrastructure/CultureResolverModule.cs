namespace Chiffon.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure.Addressing;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    public class CultureResolverModule : IHttpModule
    {
        static readonly CultureInfo EnglishCulture_ = new CultureInfo("en-US");
        static readonly CultureInfo EnglishUICulture_ = new CultureInfo("en");

        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.BeginRequest += OnBeginRequest;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void SelfRegister()
        {
            DynamicModuleUtility.RegisterModule(typeof(CultureResolverModule));
        }

        // WARNING: Cette méthode ne convient pas avec les contrôleurs asynchrones car on change de Thread.
        void OnBeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            // TODO: Utiliser SiteMap
            if (app.Request.Url.Host.StartsWith("en.")) {
                // Culture utilisée par ResourceManager.
                Thread.CurrentThread.CurrentUICulture = EnglishUICulture_;
                // Culture utilisée par System.Globalization.
                Thread.CurrentThread.CurrentCulture = EnglishCulture_;
            }

            // FIXME: Explorer les codes suivants pour activer l'IoC pour les modules HTTP.
            // MVC Turbine ?
            // HttpModuleMagic ?
            var siteMap = DependencyResolver.Current.GetService<ISiteMapFactory>()
                .CreateMap(ChiffonCulture.CurrentUICulture);

            app.Context.SetSiteMap(siteMap);
        }
    }

    public enum ChiffonLanguage
    {
        Default = French,
        French = 0,
        English = 1,
    }

    public static class ChiffonCulture
    {
        static readonly CultureInfo DefaultCulture_ = new CultureInfo("fr-FR");
        static readonly CultureInfo DefaultUICulture_ = new CultureInfo("fr");
        static readonly CultureInfo EnglishCulture_ = new CultureInfo("en-US");
        static readonly CultureInfo EnglishUICulture_ = new CultureInfo("en");

        public static CultureInfo CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture; }
        }

        public static CultureInfo CurrentUICulture
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
        }

        public static string CurrentLanguage
        {
            get { return CurrentUICulture.TwoLetterISOLanguageName;}
        }

        public static ChiffonLanguage ResolveCultureForRequest(HttpRequest request)
        {
            if (request.Url.Host.StartsWith("en.")) {
                return ChiffonLanguage.English;
            }
            else {
                return ChiffonLanguage.Default;
            }
        }

        public static void SetThreadCulture(ChiffonLanguage culture)
        {
            if (culture == ChiffonLanguage.Default) {
                return;
            }

            // Culture utilisée par ResourceManager.
            Thread.CurrentThread.CurrentUICulture = EnglishUICulture_;
            // Culture utilisée par System.Globalization.
            Thread.CurrentThread.CurrentCulture = EnglishCulture_;
        }
    }
}
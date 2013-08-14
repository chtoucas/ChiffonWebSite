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

    public class ThreadCultureModule : IHttpModule
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
            DynamicModuleUtility.RegisterModule(typeof(ThreadCultureModule));
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
                .CreateMap(Thread.CurrentThread.CurrentUICulture);

            SiteMapUtility.SetSiteMap(app.Context, siteMap);
        }
    }
}
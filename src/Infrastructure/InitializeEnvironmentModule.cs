namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure.Addressing;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    public class InitializeEnvironmentModule : IHttpModule
    {
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

        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(InitializeEnvironmentModule));
        }

        // WARNING: Cette méthode ne convient pas avec les actions asynchrones car on peut changer de Thread.
        void OnBeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            // FIXME: Explorer les librairies suivantes afin d'activer l'injection de dépendances
            // pour les modules HTTP.
            // MVC Turbine ?
            // HttpModuleMagic ?
            var resolver = DependencyResolver.Current;
            var config = resolver.GetService<ChiffonConfig>();
            var siteMapFactory = resolver.GetService<ISiteMapFactory>();

            var environment = ChiffonEnvironment.ResolveAndInitialize(config, app.Request);
            var siteMap = siteMapFactory.CreateMap(environment);

            app.Context.SetSiteMap(siteMap);
        }
    }
}
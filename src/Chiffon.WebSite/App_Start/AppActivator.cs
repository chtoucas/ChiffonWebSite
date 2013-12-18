﻿using Chiffon;
using WebActivatorEx;

// WARNING: cet attribut ne peut être utilisé qu'une fois par assemblée.
//[assembly: System.Web.PreApplicationStartMethod(typeof(AppActivator), "PreStart")]

[assembly: PreApplicationStartMethod(typeof(AppActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(AppActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(AppActivator), "Shutdown")]

namespace Chiffon
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.Modules;
    using Narvalo.Web;
    using Serilog;

    public static class AppActivator
    {
        public static void PreStart()
        {
            // Chargement de la configuration.
            var config = ChiffonConfig.FromConfiguration();

            // Résolution des dépendances (Autofac).
            var builder = new ContainerBuilder();
            builder.RegisterModule(new InfrastructureModule(config));
            builder.RegisterModule(new DataModule(config));
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new AspNetMvcModule());
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Configuration du logger (Serilog).
            (new LogConfig(config)).Configure();

            // Modules HTTP.
            HttpHeaderCleanupModule.Register();
            HttpHeaderPolicyModule.Register();
            ApplicationLifecycleModule.Register();
            InitializeContextModule.Register();

            // Supprime l'en-tête "X-AspNetMvc-Version".
            MvcHandler.DisableMvcResponseHeader = true;
        }

        public static void Start()
        {
            Log.Information("Application starting.");

            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(DesignerKey), new DesignerKeyModelBinder());

            ResetViewEngines_();
        }

        public static void PostStart()
        {
            ;
        }

        public static void End()
        {
            Log.Information("Application ending.");
        }

        /// <summary>
        /// Méthode exécutée après le déchargement du dernier module HTTP. 
        /// </summary>
        public static void Shutdown()
        {
            ;
        }

        #region Méthodes privées

        static void ResetViewEngines_()
        {
            // On ne garde que Razor.
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        #endregion
    }
}

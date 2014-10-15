using Chiffon;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(AppActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(AppActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(AppActivator), "Shutdown")]

namespace Chiffon
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure;
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
            builder.RegisterModule(new MediaSiteModule(config));
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Configuration du logger (Serilog).
            (new LogConfig(config)).Configure();

            // Modules HTTP.
            ApplicationLifecycleModule.Register();

            // Supprime l'en-tête "X-AspNetMvc-Version".
            MvcHandler.DisableMvcResponseHeader = true;
        }

        public static void Start()
        {
            Log.Information("Application starting.");

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public static void PostStart()
        {
            ;
        }

        public static void End()
        {
            Log.Information("Application ending.");
        }

        public static void Shutdown()
        {
            ;
        }
    }
}

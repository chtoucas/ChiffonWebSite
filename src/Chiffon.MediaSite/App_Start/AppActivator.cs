using Chiffon;
using WebActivatorEx;

// WARNING: cet attribut ne peut être utilisé qu'une fois par assemblée.
//[assembly: System.Web.PreApplicationStartMethod(typeof(AppActivator), "PreStart")]

[assembly: PreApplicationStartMethod(typeof(AppActivator), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(AppActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(AppActivator), "Shutdown")]

namespace Chiffon
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure;
    using Narvalo.Web;

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
            (new LogConfigurator(config)).Configure();

            // Modules HTTP.
            HttpHeaderCleanupModule.Register();
            HttpHeaderPolicyModule.Register();

            // Supprime l'en-tête "X-AspNetMvc-Version".
            MvcHandler.DisableMvcResponseHeader = true;

            //PreStartMiniProfiler_();
        }

        public static void PostStart()
        {
            //PostStartMiniProfiler_();
        }

        /// <summary>
        /// Méthode exécutée après le déchargement du dernier module HTTP. 
        /// </summary>
        public static void Shutdown()
        {
            ;
        }
    }
}

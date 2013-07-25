namespace Chiffon.WebSite
{
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.WebSite.Configs;
    using Narvalo.Web;
    //using StackExchange.Profiling.MVCHelpers;

    public static class AppActivator
    {
        public static void PreStart()
        {
            // Configuration de log4net.
            log4net.Config.XmlConfigurator.Configure();

            // Modules HTTP.
            HttpHeaderCleanupModule.SelfRegister();

            //PreStartMiniProfiler_();

            // Configuration d'Autofac.
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CrossCuttingsModule());
            builder.RegisterModule(new InfrastructureModule());

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
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

        //#region Méthodes privées

        //[Conditional("PROFILE")]
        //static void PreStartMiniProfiler_()
        //{
        //    MiniProfilerModule.SelfRegister();
        //}

        //[Conditional("PROFILE")]
        //static void PostStartMiniProfiler_()
        //{
        //    // Intercept ViewEngines to profile all partial views and regular views.
        //    // If you prefer to insert your profiling blocks manually you can comment this out
        //    var copy = ViewEngines.Engines.ToList();
        //    ViewEngines.Engines.Clear();
        //    foreach (var item in copy) {
        //        ViewEngines.Engines.Add(new ProfilingViewEngine(item));
        //    }
        //}

        //#endregion
    }
}

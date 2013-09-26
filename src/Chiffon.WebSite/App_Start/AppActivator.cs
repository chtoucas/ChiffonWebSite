namespace Chiffon
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Chiffon.Infrastructure;
    using Narvalo.Web;
    //using StackExchange.Profiling.MVCHelpers;

    public static class AppActivator
    {
        public static void PreStart()
        {
            // Chargement de la configuration.
            var config = ChiffonConfig.FromConfiguration();

            // Résolution des dépendances (Autofac).
            var builder = new ContainerBuilder();
            builder.RegisterModule(new WebSiteModule(config));
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Configuration du logger (Serilog).
            (new LogConfigurator(config)).Configure();

            // Modules HTTP.
            HttpHeaderCleanupModule.Register();
            HttpHeaderPolicyModule.Register();
            InitializeRuntimeModule.Register();

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

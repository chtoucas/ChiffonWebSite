namespace Chiffon.WebSite
{
    using System.Web.Mvc;
    using Narvalo.Web;
    //using StackExchange.Profiling.MVCHelpers;

    public static class AppActivator
    {
        public static void PreStart()
        {
            // Configuration de log4net.
            //log4net.Config.XmlConfigurator.Configure();

            // Modules HTTP.
            HttpHeaderCleanupModule.SelfRegister();

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

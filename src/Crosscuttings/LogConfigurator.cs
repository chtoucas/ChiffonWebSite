namespace Chiffon.Crosscuttings
{
    using System.ComponentModel.Composition.Hosting;
    using Serilog;
    //using Serilog.Web;

    public static class LogConfigurator
    {
        public static void Configure(ChiffonConfig config)
        {
            ConfigureSerilog_(config);
        }

        // Configuration de Serilog.
        // NB: Pour le moment, le principal défaut de Serilog est l'absence de configuration 
        // à l'exécution. Pistes à explorer :
        // - utiliser les facilités offertes par App_Code, vraiment pas sûr que cela marche ;
        // - utiliser MEF.
        static void ConfigureSerilog_(ChiffonConfig config)
        {
            // Si Serilog.Web est installé, ajouter la ligne suivante :
            //ApplicationLifecycleModule.IsEnabled = false;

            ILogService svc;

            using (var catalog = new AssemblyCatalog(typeof(Global).Assembly)) {
                using (var container = new CompositionContainer(catalog)) {
                    svc = container.GetExportedValue<ILogService>(config.LogConfig);
                }
            }

            Log.Logger = svc.GetLogger(config.LogMinimumLevel);
        }

        //// Configuration de log4net.
        //static void ConfigureLog4net_()
        //{
        //    log4net.Config.XmlConfigurator.Configure();
        //}
    }
}
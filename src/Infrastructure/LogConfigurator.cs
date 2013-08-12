namespace Chiffon.Infrastructure
{
    using System.ComponentModel.Composition.Hosting;
    using Narvalo;
    using Serilog;
    //using Serilog.Web;

    public class LogConfigurator
    {
        readonly ChiffonConfig _config;

        public LogConfigurator(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        // Configuration de Serilog.
        // NB: Pour le moment, le principal défaut de Serilog est l'absence de configuration 
        // à l'exécution. Pistes à explorer :
        // - utiliser les facilités offertes par App_Code, vraiment pas sûr que cela marche ;
        // - utiliser MEF.
        public void Configure()
        {
            // Si Serilog.Web est installé, ajouter la ligne suivante :
            //ApplicationLifecycleModule.IsEnabled = false;

            ILogService svc;

            using (var catalog = new AssemblyCatalog(typeof(Global).Assembly)) {
                using (var container = new CompositionContainer(catalog)) {
                    svc = container.GetExportedValue<ILogService>(_config.LogProfile);
                }
            }

            Log.Logger = svc.GetLogger(_config.LogMinimumLevel);
        }

        //// Configuration de log4net.
        //public void Configure()
        //{
        //    log4net.Config.XmlConfigurator.Configure();
        //}
    }
}
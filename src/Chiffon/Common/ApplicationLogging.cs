namespace Chiffon.Common
{
    using System.ComponentModel.Composition.Hosting;

    using Narvalo;
    using Serilog;

    public sealed class ApplicationLogging
    {
        private readonly ChiffonConfig _config;

        public ApplicationLogging(ChiffonConfig config)
        {
            Require.NotNull(config, "config");

            _config = config;
        }

        // Configuration de Serilog.
        // NB: Pour le moment, le principal défaut de Serilog est l'absence de configuration 
        // à l'exécution. Pistes à explorer :
        // - utiliser les facilités offertes par App_Code, vraiment pas sûr que cela marche ;
        // - utiliser MEF.
        public void Configure()
        {
            ILogService svc;

            using (var catalog = new AssemblyCatalog(typeof(ApplicationLogging).Assembly)) {
                using (var container = new CompositionContainer(catalog)) {
                    svc = container.GetExportedValue<ILogService>(_config.LogProfile);
                }
            }

            Log.Logger = svc.GetLogger(_config.LogMinimumLevel);
        }
    }
}
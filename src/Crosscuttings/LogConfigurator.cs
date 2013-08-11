namespace Chiffon.Crosscuttings
{
    using System;
    using System.IO;
    using Serilog;
    //using Serilog.Web;
    //using Serilog.Web.Enrichers;

    public static class LogConfigurator
    {
        public static void Configure()
        {
            ConfigureSerilog_();
        }

        // Configuration de Serilog.
        // NB: Pour le moment, le principal défaut de Serilog est l'absence de configuration à l'exécution.
        // Possibles solutions à explorer :
        // - utiliser les facilités offertes par App_Code, vraiment pas sûr que cela marche ;
        // - utiliser MEF.
        static void ConfigureSerilog_()
        {
            // Si Serilog.Web est installé, ajouter la ligne suivante :
            //ApplicationLifecycleModule.IsEnabled = false;

            // Cf. http://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
            var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var logfile = Path.Combine(dataDirectory, "Logs", "log.txt");
            //var rollingFile = Path.Combine(dataDirectory, "Logs", "log-{Date}.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                //.WriteTo.Trace(outputTemplate: "{Timestamp} [{Level}] ({HttpRequestId}) {Message:l}{NewLine:l}{Exception:l}")
                .WriteTo.File(logfile)
                //.WriteTo.RollingFile(logfile, fileSizeLimitBytes: 1024, retainedFileCountLimit: 7)
                //.Enrich.With<HttpRequestIdEnricher>()
                .CreateLogger();
        }

        // Configuration de log4net.
        static void ConfigureLog4net_()
        {
            //log4net.Config.XmlConfigurator.Configure();
        }
    }
}
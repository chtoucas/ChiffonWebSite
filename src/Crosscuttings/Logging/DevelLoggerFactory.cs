namespace Chiffon.Crosscuttings.Logging
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using Serilog;
    //using Serilog.Web.Enrichers;

    [Export("Development", typeof(ILoggerFactory))]
    public class DevelLoggerFactory : ILoggerFactory
    {
        readonly ChiffonConfig _config;

        [ImportingConstructor]
        public DevelLoggerFactory([Import("Config")] ChiffonConfig config)
        {
            _config = config;
        }

        #region ILoggerFactory

        public ILogger CreateLogger()
        {
            // Cf. http://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
            var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var logfile = Path.Combine(dataDirectory, "Logs", "log.txt");

            ILogger logger = new LoggerConfiguration()
                .SetMinimumLevel(_config.LoggerLevel)
                .WriteTo.File(logfile)
                //.Enrich.With<HttpRequestIdEnricher>()
                .CreateLogger();

            return logger;
        }

        #endregion
    }
}
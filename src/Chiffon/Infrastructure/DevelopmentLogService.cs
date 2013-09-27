﻿namespace Chiffon.Infrastructure
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using Serilog;
    using Serilog.Events;

    [Export("Development", typeof(ILogService))]
    public class DevelopmentLogService : ILogService
    {
        #region ILogService

        [CLSCompliant(false)]
        public ILogger GetLogger(LogEventLevel minimumLevel)
        {
            // Cf. http://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
            var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var logfile = Path.Combine(dataDirectory, "Logs", "log.txt");

            return new LoggerConfiguration()
                .MinimumLevel.Is(minimumLevel)
                // On écrit dans un fichier de taille maximale 1Mo.
                .WriteTo.File(logfile, fileSizeLimitBytes: 1048576)
                .Enrich.With<HttpRequestEnricher>()
                .CreateLogger();
        }

        #endregion
    }
}
﻿namespace Chiffon.Common
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    using Serilog;
    using Serilog.Events;

    [Export("Development", typeof(ILogService))]
    public sealed class DevelopmentLogService : ILogService
    {
        [CLSCompliant(false)]
        public ILogger GetLogger(LogEventLevel minimumLevel)
        {
            // Cf. http://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
            var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var logfile = Path.Combine(dataDirectory, "Logs", "log.txt");

            // Si Serilog.Extras.Web est installé, le module ApplicationLifecycleModule est automatiquement
            // ajouté, on peut le désactiver en ajoutant la ligne suivante :
            //Serilog.Extras.Web.ApplicationLifecycleModule.IsEnabled = false;

            return new LoggerConfiguration()
                .MinimumLevel.Is(minimumLevel)
                // On écrit dans un fichier de taille maximale 1Mo.
                .WriteTo.File(logfile, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] ({RawUrl}) {Message:l}{NewLine:l}{Exception:l}", fileSizeLimitBytes: 1048576)
                // On écrit aussi une trace qui apparaîtra dans Glimpse.
                .WriteTo.Trace(outputTemplate: "{Timestamp} [{Level}] ({RawUrl}) {Message:l}{NewLine:l}{Exception:l}")
                .Enrich.With<HttpLogEventEnricher>()
                .CreateLogger();
        }
    }
}
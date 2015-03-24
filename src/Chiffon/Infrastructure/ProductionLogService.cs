namespace Chiffon.Infrastructure
{
    using System;
    using System.ComponentModel.Composition;

    using Narvalo.Externs.Serilog;
    using Narvalo.Web;
    using Serilog;
    using Serilog.Events;

    [Export("Production", typeof(ILogService))]
    public class ProductionLogService : ILogService
    {
        [CLSCompliant(false)]
        public ILogger GetLogger(LogEventLevel minimumLevel)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(minimumLevel)
                .WriteTo.Trace(outputTemplate: "{Timestamp} [{Level}] ({RawUrl}) {Message:l}{NewLine:l}{Exception:l}")
                .Enrich.With<HttpLogEventEnricher>()
                .CreateLogger();
        }
    }
}
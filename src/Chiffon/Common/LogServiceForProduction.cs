namespace Chiffon.Common
{
    using System;
    using System.ComponentModel.Composition;

    using Serilog;
    using Serilog.Events;

    [Export("Production", typeof(ILogService))]
    public sealed class LogServiceForProduction : ILogService
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
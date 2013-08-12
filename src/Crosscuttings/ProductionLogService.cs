namespace Chiffon.Crosscuttings
{
    using System.ComponentModel.Composition;
    using Serilog;
    using Serilog.Events;

    [Export("Production", typeof(ILogService))]
    public class ProductionLogService : ILogService
    {
        #region ILogService

        public ILogger GetLogger(LogEventLevel minimumLevel)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(minimumLevel)
                .WriteTo.Trace(outputTemplate: "{Timestamp} [{Level}] ({HttpRequestId}) {Message:l}{NewLine:l}{Exception:l}")
                .Enrich.With<HttpRequestEnricher>()
                .CreateLogger();
        }

        #endregion
    }
}
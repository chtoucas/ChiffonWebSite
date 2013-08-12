namespace Chiffon.Crosscuttings.Logging
{
    using System.ComponentModel.Composition;
    using Serilog;
    //using Serilog.Web.Enrichers;

    [Export("Production", typeof(ILoggerFactory))]
    public class ProdLoggerFactory : ILoggerFactory
    {
        readonly ChiffonConfig _config;

        [ImportingConstructor]
        public ProdLoggerFactory([Import("Config")] ChiffonConfig config)
        {
            _config = config;
        }

        #region ILoggerFactory

        public ILogger CreateLogger()
        {
            ILogger logger = new LoggerConfiguration()
                .SetMinimumLevel(_config.LoggerLevel)
                .WriteTo.Trace(outputTemplate: "{Timestamp} [{Level}] ({HttpRequestId}) {Message:l}{NewLine:l}{Exception:l}")
                //.Enrich.With<HttpRequestIdEnricher>()
                .CreateLogger();

            return logger;
        }

        #endregion
    }
}
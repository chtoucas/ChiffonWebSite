namespace Chiffon.Crosscuttings.Logging
{
    using System;
    using Serilog;
    using Serilog.Events;

    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration SetMinimumLevel(this LoggerConfiguration self, LogEventLevel minimumLevel)
        {
            switch (minimumLevel) {
                case LogEventLevel.Debug:
                    self.MinimumLevel.Debug(); return self;
                case LogEventLevel.Error:
                    self.MinimumLevel.Error(); return self;
                case LogEventLevel.Fatal:
                    self.MinimumLevel.Fatal(); return self;
                case LogEventLevel.Information:
                    self.MinimumLevel.Information(); return self;
                case LogEventLevel.Verbose:
                    self.MinimumLevel.Verbose(); return self;
                case LogEventLevel.Warning:
                    self.MinimumLevel.Warning(); return self;
                default:
                    throw new InvalidOperationException("XXX");
            }
        }
    }

}
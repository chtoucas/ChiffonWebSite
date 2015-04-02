namespace Chiffon.Common
{
    using System;

    using Serilog;
    using Serilog.Events;

    [CLSCompliant(false)]
    public interface ILoggerProvider
    {
        ILogger GetLogger(LogEventLevel minimumLevel);
    }
}
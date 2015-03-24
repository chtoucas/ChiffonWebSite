namespace Chiffon.Infrastructure
{
    using System;

    using Serilog;
    using Serilog.Events;

    [CLSCompliant(false)]
    public interface ILogService
    {
        ILogger GetLogger(LogEventLevel minimumLevel);
    }
}
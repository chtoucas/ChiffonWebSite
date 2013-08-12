namespace Chiffon.Crosscuttings
{
    using Serilog;
    using Serilog.Events;

    public interface ILogService
    {
        ILogger GetLogger(LogEventLevel minimumLevel);
    }

}
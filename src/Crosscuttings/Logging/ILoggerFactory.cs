namespace Chiffon.Crosscuttings.Logging
{
    using Serilog;

    public interface ILoggerFactory
    {
        ILogger CreateLogger();
    }

}
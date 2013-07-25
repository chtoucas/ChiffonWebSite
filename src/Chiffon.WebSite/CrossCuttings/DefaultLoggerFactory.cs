namespace Chiffon.WebSite.CrossCuttings
{
    using System;
    using log4net;
    using Narvalo.Diagnostics;

    public static class DefaultLoggerFactory
    {
        public static ILogger CreateLogger(Type type)
        {
            return new Log4NetProxy(LogManager.GetLogger(type));
        }

        public static ILogger CreateLogger(string name)
        {
            return new Log4NetProxy(LogManager.GetLogger(name));
        }
    }
}

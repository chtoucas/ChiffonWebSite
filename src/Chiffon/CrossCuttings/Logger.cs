namespace Chiffon.CrossCuttings
{
    using System;
    using Narvalo.Diagnostics;
    using Narvalo.Log4Net;

    public static class Logger
    {
        static ILoggerFactory Factory_ = new Log4NetFactory();

        public static ILogger Create(Type type)
        {
            return Factory_.CreateLogger(type);
        }

        public static ILogger Create(string name)
        {
            return Factory_.CreateLogger(name);
        }
    }
}

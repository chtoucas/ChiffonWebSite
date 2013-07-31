namespace Chiffon.CrossCuttings
{
    using System;
    using Narvalo.Diagnostics;

    public static class Logger
    {
        static ILoggerFactory Factory_ = new NoopLoggerFactory();

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

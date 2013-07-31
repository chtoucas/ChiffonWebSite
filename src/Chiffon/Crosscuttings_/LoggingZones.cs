namespace Chiffon.Crosscuttings
{
    using System;
    using Narvalo.Diagnostics;

    public static class LoggingZones
    {
        //static Lazy<ILogger> ApplicationLogger_
        //    = new Lazy<ILogger>(() => Logger.Create("Application"));
        static Lazy<ILogger> CrossCuttingsLogger_
            = new Lazy<ILogger>(() => Logger.Create("CrossCuttings"));
        static Lazy<ILogger> InfrastructureLogger_
            = new Lazy<ILogger>(() => Logger.Create("Infrastructure"));
        static Lazy<ILogger> PersistenceLogger_
            = new Lazy<ILogger>(() => Logger.Create("Persistence"));

        //public static ILogger Application
        //{
        //    get { return ApplicationLogger_.Value; }
        //}

        public static ILogger CrossCuttings
        {
            get { return CrossCuttingsLogger_.Value; }
        }

        public static ILogger Infrastructure
        {
            get { return InfrastructureLogger_.Value; }
        }

        public static ILogger Persistence
        {
            get { return PersistenceLogger_.Value; }
        }
    }
}

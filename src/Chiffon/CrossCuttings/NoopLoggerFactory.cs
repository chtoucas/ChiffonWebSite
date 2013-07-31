namespace Chiffon.CrossCuttings
{
    using System;
    using Narvalo.Diagnostics;

    public class NoopLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type)
        {
            return new NoopLogger(type.Name);
        }

        public ILogger CreateLogger(string name)
        {
            return new NoopLogger(name);
        }
    }
}

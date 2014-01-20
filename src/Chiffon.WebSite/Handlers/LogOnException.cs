namespace Chiffon.Handlers
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class LogOnException : Exception
    {
        public LogOnException() : base() { }

        public LogOnException(string message) : base(message) { ; }

        public LogOnException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected LogOnException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
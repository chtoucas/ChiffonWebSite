namespace Chiffon.Handlers
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class PatternImageException : Exception
    {
        public PatternImageException() : base() { }

        public PatternImageException(string message) : base(message) { ; }

        public PatternImageException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected PatternImageException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
namespace Chiffon.WebSite.CrossCuttings
{
    using System;
    //using System.Diagnostics;
    using log4net;
    using Narvalo.Diagnostics;

    public sealed class Log4NetProxy : ILogger
    {
        private readonly ILog _inner;

        internal Log4NetProxy(ILog inner)
        {
            _inner = inner;
        }

        #region ILogger

        public bool IsDebugEnabled
        {
            get { return _inner.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _inner.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _inner.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _inner.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _inner.IsWarnEnabled; }
        }

        //[Conditional("DEBUG")]
        public void Debug(object message)
        {
            _inner.Debug(message);
        }

        //[Conditional("DEBUG")]
        public void Debug(object message, Exception exception)
        {
            _inner.Debug(message, exception);
        }

        //[Conditional("DEBUG")]
        public void Debug(IFormatProvider provider, string format, params object[] args)
        {
            _inner.DebugFormat(provider, format, args);
        }

        public void Error(object message)
        {
            _inner.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _inner.Error(message, exception);
        }

        public void Error(IFormatProvider provider, string format, params object[] args)
        {
            _inner.ErrorFormat(provider, format, args);
        }

        public void Fatal(object message)
        {
            _inner.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _inner.Fatal(message, exception);
        }

        public void Fatal(IFormatProvider provider, string format, params object[] args)
        {
            _inner.FatalFormat(provider, format, args);
        }

        public void Info(object message)
        {
            _inner.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _inner.Info(message, exception);
        }

        public void Info(IFormatProvider provider, string format, params object[] args)
        {
            _inner.InfoFormat(provider, format, args);
        }

        public void Warn(object message)
        {
            _inner.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _inner.Warn(message, exception);
        }

        public void Warn(IFormatProvider provider, string format, params object[] args)
        {
            _inner.WarnFormat(provider, format, args);
        }

        #endregion
    }
}
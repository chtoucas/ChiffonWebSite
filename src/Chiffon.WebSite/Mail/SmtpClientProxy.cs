namespace Chiffon.Mail
{
    using System;
    using System.Net.Mail;
    using Narvalo;

    public class SmtpClientProxy : ISmtpClient
    {
        bool _disposed = false;
        SmtpClient _inner;

        public SmtpClientProxy()
        {
            _inner = new SmtpClient();
        }

        public SmtpClientProxy(SmtpClient smtpClient)
        {
            _inner = smtpClient;
        }

        #region ISmtpClient

        public event EventHandler<EmailSentEventArgs> SentEventHandler;

        // FIXME
        public bool IsConnected { get { return _inner != null; } }

        public void Connect()
        {
            ;
        }

        public void Disconnect()
        {
            ;
        }

        public void Send(MailMessage message)
        {
            Requires.NotNull(message, "message");

            ThrowIfNotConnected();

            _inner.Send(message);

            OnSent(new EmailSentEventArgs(message));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    _inner.Dispose();
                    _inner = null;
                }

                _disposed = true;
            }
        }

        protected virtual void OnSent(EmailSentEventArgs e)
        {
            var localHandler = SentEventHandler;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

        protected void ThrowIfNotConnected()
        {
            if (_disposed) {
                throw new ObjectDisposedException("Cannot use a closed connection.");
            }

            if (_inner == null) {
                throw new SmtpClientException("Before callings this method, you must first call Connect().");
            }
        }
    }
}

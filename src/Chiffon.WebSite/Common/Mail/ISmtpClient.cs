namespace Chiffon.Mail
{
    using System;
    using System.Net.Mail;

    public interface ISmtpClient : IDisposable
    {
        event EventHandler<EmailSentEventArgs> SentEventHandler;

        bool IsConnected { get; }

        void Connect();
        void Disconnect();

        void Send(MailMessage message);
    }
}

namespace Chiffon.Mail
{
    using System;
    using System.Net.Mail;

    public class EmailSentEventArgs : EventArgs
    {
        public EmailSentEventArgs(MailMessage message)
        {
            Message = message;
        }

        public MailMessage Message
        {
            get;
            private set;
        }
    }
}

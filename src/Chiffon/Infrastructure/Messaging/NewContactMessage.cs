namespace Chiffon.Infrastructure.Messaging
{
    using System.Net.Mail;

    public class NewContactMessage
    {
        public MailAddress EmailAddress { get; set; }

        public string MessageContent { get; set; }
    }
}

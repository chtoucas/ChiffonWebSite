namespace Chiffon.Infrastructure.Messaging
{
    using System.Net.Mail;

    public class NewContactMessage
    {
        public MailAddress ContactAddress { get; set; }
        public string Content { get; set; }
    }
}

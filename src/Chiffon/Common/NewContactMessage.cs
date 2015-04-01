namespace Chiffon.Common
{
    using System.Net.Mail;

    public sealed class NewContactMessage
    {
        public MailAddress EmailAddress { get; set; }

        public string MessageContent { get; set; }
    }
}

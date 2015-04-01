namespace Chiffon.Common
{
    using System.Net.Mail;

    public sealed class NewMemberMessage
    {
        public string CompanyName { get; set; }

        public MailAddress EmailAddress { get; set; }

        public string Password { get; set; }

        public MessageRecipients Recipients { get; set; }
    }
}

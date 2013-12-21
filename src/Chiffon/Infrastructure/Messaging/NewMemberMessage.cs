namespace Chiffon.Infrastructure.Messaging
{
    using System.Net.Mail;

    public class NewMemberMessage
    {
        public MailAddress MemberAddress { get; set; }
        public string CompanyName { get; set; }
        public string Password { get; set; }
    }
}

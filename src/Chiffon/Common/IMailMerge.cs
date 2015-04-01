namespace Chiffon.Common
{
    using System.Net.Mail;


    public interface IMailMerge
    {
        MailMessage WelcomeMail(NewMemberMessage message);

        MailMessage NewContactNotification(NewContactMessage message);

        MailMessage NewMemberNotification(NewMemberMessage message);
    }
}

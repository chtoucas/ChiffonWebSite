namespace Chiffon.Infrastructure.Messaging
{
    using System.Net.Mail;
    using Narvalo;

    public class Messenger/*Impl*/ : IMessenger
    {
        readonly IMailMerge _mailMerge;

        public Messenger(IMailMerge mailMerge)
        {
            Requires.NotNull(mailMerge, "mailMerge");

            _mailMerge = mailMerge;
        }

        // Envoi de l'email de confirmation d'inscription ainsi que d'une notification au administrateurs.
        public void Publish(NewMemberMessage message)
        {
            var mail = _mailMerge.WelcomeMail(message);
            var notification = _mailMerge.NewMemberNotification(message);

            using (var smtpClient = new SmtpClient()) {
                smtpClient.Send(mail);
                smtpClient.Send(notification);
            }
        }

        public void Publish(NewContactMessage message)
        {
            var notification = _mailMerge.NewContactNotification(message);

            using (var smtpClient = new SmtpClient()) {
                smtpClient.Send(notification);
            }
        }
    }
}

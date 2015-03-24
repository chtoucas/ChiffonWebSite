namespace Chiffon.Infrastructure.Messaging
{
    using System.Net.Mail;

    using Narvalo;
    using Serilog;

    public class Messenger/*Impl*/ : IMessenger
    {
        readonly IMailMerge _mailMerge;

        public Messenger(IMailMerge mailMerge)
        {
            Require.NotNull(mailMerge, "mailMerge");

            _mailMerge = mailMerge;
        }

        #region IMessenger

        /// <summary>
        /// Envoi des notifications lors de l'inscription d'un nouveau membre.
        /// </summary>
        /// <param name="message">Modèle du message à envoyer.</param>
        public void Publish(NewMemberMessage message)
        {
            Require.NotNull(message, "message");

            SmtpClient smtpClient = null;

            try
            {
                smtpClient = new SmtpClient();

                // On envoie d'abord le mail destiné au nouveau membre (le plus important).
                if (message.Recipients.Contains(MessageRecipients.Member))
                {
                    using (var mail = _mailMerge.WelcomeMail(message))
                    {
                        smtpClient.Send(mail);
                    }
                }

                if (message.Recipients.Contains(MessageRecipients.Admin))
                {
                    using (var notification = _mailMerge.NewMemberNotification(message))
                    {
                        smtpClient.Send(notification);
                    }
                }
            }
            catch (SmtpException ex)
            {
                // NB: On ne "rethrow" pas les exceptions SMTP.
                // Si une erreur intervient lors du message au nouveau membre, il n'aura
                // pas de mail contenant son mot de passe mais pourra le récupérer sur le site
                // via le lien "Mot de passe oublié".
                Log.Error(ex, "A SMTP error occured while sending the new member messages.");
            }
            finally
            {
                if (smtpClient != null)
                {
                    smtpClient.Dispose();
                }
            }
        }

        /// <summary>
        /// Envoi des notifications lors d'une demande effectuée à partir de la page contact.
        /// </summary>
        /// <param name="message">Modèle de message</param>
        public void Publish(NewContactMessage message)
        {
            using (var smtpClient = new SmtpClient())
            {
                using (var notification = _mailMerge.NewContactNotification(message))
                {
                    smtpClient.Send(notification);
                }
            }
        }

        #endregion
    }
}

namespace Chiffon.Services
{
    using System;

    using Chiffon.Infrastructure.Messaging;

    /// <summary>
    /// Représente une demande d'inscription associée à la méthode
    /// <see cref="Chiffon.Services.IMemberService.RegisterMember"/>.
    /// </summary>
    [Serializable]
    public class RegisterMemberRequest
    {
        MessageRecipients _recipients = MessageRecipients.Default;

        /// <summary>
        /// Assigne ou retourne le nom de l'agence à laquelle appartient le membre à créer.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Assigne ou retourne l'e-mail du membre à créer.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Assigne ou retourne le prénom du membre à créer.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Assigne ou retourne le nom de famille du membre à créer.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Assigne ou retourne une valeur indiquant si le membre souhaite recevoir la newsletter.
        /// </summary>
        public bool NewsletterChecked { get; set; }

        /// <summary>
        /// Destinataires des notifications envoyées une fois la création du membre effectuée.
        /// Par défaut, on envoie à tout le monde.
        /// </summary>
        public MessageRecipients Recipients
        {
            get { return _recipients; }
            set { _recipients = value; }
        }
    }
}

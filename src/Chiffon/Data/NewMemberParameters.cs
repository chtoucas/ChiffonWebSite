namespace Chiffon.Data
{
    using System;

    /// <summary>
    /// Représente les paramètres nécessaires à la création d'un membre et utilisés par
    /// <see cref="Chiffon.Data.IDbCommands.NewMember"/>.
    /// </summary>
    [Serializable]
    public class NewMemberParameters
    {
        /// <summary>
        /// Assigne ou retourne le nom de l'agence à laquelle le membre appartient.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Assigne ou retourne le mot de passe encrypté du membre.
        /// </summary>
        public string EncryptedPassword { get; set; }

        /// <summary>
        /// Assigne ou retourne l'e-mail du membre.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Assigne ou retourne le prénom du membre.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Assigne ou retourne le nom de famille du membre.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Assigne ou retourne une valeur indiquant si le membre souhaite recevoir la newsletter.
        /// </summary>
        public bool NewsletterChecked { get; set; }
    }
}

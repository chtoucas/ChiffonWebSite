namespace Chiffon.Entities
{
    using System;
    using System.Net.Mail;
    using Narvalo.Fx;

    /// <summary>
    /// Représente un designer.
    /// </summary>
    [Serializable]
    public class Designer
    {
        readonly DesignerKey _key;

        [NonSerialized]
        string _displayName;
        [NonSerialized]
        MailAddress _emailAddress;

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Chiffon.Entities.Designer"/>.
        /// </summary>
        /// <param name="key">Clé identifiant de manière unique le designer.</param>
        public Designer(DesignerKey key)
        {
            _key = key;
        }

        /// <summary>
        /// Clé identifiant de manière unique le designer.
        /// </summary>
        /// <remarks>
        /// Cette propriété va être aussi utilisée pour construire :
        /// - le nom du répertoire dans lequel les motifs du designer sont stockés ;
        /// - le filtre d'identification dans les URLs.
        /// </remarks>
        public DesignerKey Key { get { return _key; } }

        public string AvatarCategory { get; set; }

        public string AvatarReference { get; set; }

        public string AvatarVersion { get; set; }

        /// <summary>
        /// Retourne le nom du membre.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(_displayName)) {
                    // NB: Le nom affiché ne dépend pas de la culture en cours d'utilisation.
                    _displayName = Nickname.ValueOrElse(FirstName + " " + LastName);
                }
                return _displayName;
            }
        }

        /// <summary>
        /// Assigne ou retourne l'adresse e-mail du designer.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Retourne l'adresse électronique du designer construite à partir de l'e-mail
        /// et du nom du membre.
        /// </summary>
        public MailAddress EmailAddress
        {
            get
            {
                if (_emailAddress == null) {
                    _emailAddress = new MailAddress(Email, DisplayName);
                }
                return _emailAddress;
            }
        }

        /// <summary>
        /// Assigne ou retourne le prénom du designer.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Assigne ou retourne le nom de famille du designer.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Assigne ou retourne le pseudo du designer si il existe.
        /// </summary>
        public Maybe<string> Nickname { get; set; }

        /// <summary>
        /// Assigne ou retourne un texte de présentation pour le designer.
        /// </summary>
        public string Presentation { get; set; }

        /// <summary>
        /// Assigne ou retourne l'adresse du site web du designer si il existe.
        /// </summary>
        public Maybe<Uri> WebsiteUrl { get; set; }
    }
}

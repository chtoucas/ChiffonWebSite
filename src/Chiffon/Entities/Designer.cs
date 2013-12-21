namespace Chiffon.Entities
{
    using System;
    using System.Net.Mail;
    using Narvalo.Fx;

    [Serializable]
    public class Designer
    {
        readonly DesignerKey _key;
        string _displayName;
        MailAddress _emailAddress;

        public Designer(DesignerKey key)
        {
            _key = key;
        }

        /// <summary>
        /// Cette propriété va être utilisée pour trois choses :
        /// - clé d'identification unique ;
        /// - nom du répertoire dans lequel les motifs vont être stockés ;
        /// - filtre d'identification dans les URLs.
        /// </summary>
        public DesignerKey Key { get { return _key; } }

        public string AvatarCategory { get; set; }
        public string AvatarReference { get; set; }
        public string AvatarVersion { get; set; }
        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(_displayName)) {
                    _displayName = Nickname.ValueOrElse(FirstName + " " + LastName);
                }
                return _displayName;
            }
        }

        public string Email { get; set; }

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

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Maybe<string> Nickname { get; set; }
        public string Presentation { get; set; }
        public Maybe<Uri> WebsiteUrl { get; set; }
    }
}

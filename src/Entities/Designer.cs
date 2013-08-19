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
        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(_displayName)) {
                    _displayName = Nickname.ValueOrElse(Firstname + " " + Lastname);
                }
                return _displayName;
            }
        }
        public MailAddress EmailAddress { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Maybe<string> Nickname { get; set; }
        public string Presentation { get; set; }
        public Maybe<Uri> WebSiteUrl { get; set; }
    }
}

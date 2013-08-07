namespace Chiffon.Entities
{
    using System;
    using System.Net.Mail;

    [Serializable]
    public class Designer
    {
        readonly DesignerKey _key;

        Pattern _preferredPattern;

        public Designer(DesignerKey designerKey)
        {
            _key = designerKey;
        }

        /// <summary>
        /// Cette propriété va être utilisée pour trois choses :
        /// - clé d'identification unique ;
        /// - nom du répertoire dans lequel les motifs vont être stockés ;
        /// - filtre d'identification dans les URLs.
        /// </summary>
        public DesignerKey Key { get { return _key; } }

        public Pattern PreferredPattern
        {
            get
            {
                if (_preferredPattern == null) {
                    _preferredPattern = new Pattern(new PatternId(Key, PreferredPatternReference));
                }
                return _preferredPattern;
            }
        }

        public string PreferredPatternReference { get; set; }

        public string Presentation { get; set; }

        public string DisplayName { get; set; }

        public MailAddress EmailAddress { get; set; }

        public Uri Url { get; set; }
    }
}

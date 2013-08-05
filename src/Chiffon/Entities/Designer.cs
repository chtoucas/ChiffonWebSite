namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Designer
    {
        readonly DesignerKey _designerKey;

        public Designer(DesignerKey designerKey)
        {
            _designerKey = designerKey;
        }

        /// <summary>
        /// Cette propriété va être utilisée pour trois choses :
        /// - clé d'identification unique ;
        /// - nom du répertoire dans lequel les motifs vont être stockés ;
        /// - clé d'identification dans les URLs.
        /// </summary>
        public DesignerKey Key { get { return _designerKey; } }

        public string DisplayName { get; set; }
    }
}

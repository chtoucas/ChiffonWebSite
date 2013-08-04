namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Designer
    {
        readonly DesignerId _designerId;

        public Designer(DesignerId designerId)
        {
            _designerId = designerId;
        }

        public DesignerId DesignerId { get { return _designerId; } }

        public string DisplayName { get; set; }

        /// <summary>
        /// Cette propriété va être utilisée pour deux choses :
        /// - nom du répertoire dans lequel les motifs vont être stockés ;
        /// - clé d'identification dans les URLs.
        /// </summary>
        public string Key { get; set; }
    }
}

namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Category
    {
        public string Description { get; set; }

        public DesignerKey DesignerKey { get; set; }

        public string Name { get; set; }

    }
}

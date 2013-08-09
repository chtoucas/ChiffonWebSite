namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Category
    {
        public CategoryKey Key { get; set; }

        public DesignerKey DesignerKey { get; set; }

        public string DisplayName { get; set; }

        //public string Description { get; set; }
    }
}

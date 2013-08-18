namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Category
    {
        public DesignerKey DesignerKey { get; set; }

        public string DisplayName { get; set; }

        public string Key { get; set; }
    }
}

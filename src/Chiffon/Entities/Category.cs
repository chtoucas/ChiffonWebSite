namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Category
    {
        public DesignerKey DesignerId { get; set; }

        public int Id { get; set; }

        public string DisplayName { get; set; }
    }
}

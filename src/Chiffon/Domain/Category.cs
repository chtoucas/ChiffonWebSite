namespace Chiffon.Domain
{
    using System;

    [Serializable]
    public class Category
    {
        public int Id { get; set; }
        public MemberId MemberId { get; set; }
        public string Name { get; set; }
    }
}

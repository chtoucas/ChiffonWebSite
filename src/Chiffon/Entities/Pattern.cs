namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Pattern
    {
        public string Id { get; set; }

        public bool IsPrivate { get { return !IsPublic; } }

        public bool IsPublic { get; set; }

        public MemberId MemberId { get; set; }
    }
}

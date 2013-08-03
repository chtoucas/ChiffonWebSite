namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Pattern
    {
        public DesignerId DesignerId { get { return PatternId.DesignerId; } }

        public bool IsPrivate { get { return !IsPublic; } }

        public bool IsPublic { get; set; }

        public PatternId PatternId { get; set; }

        public string Reference { get { return PatternId.Reference; } }
    }
}

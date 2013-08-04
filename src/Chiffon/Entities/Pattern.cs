namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Pattern
    {
        readonly PatternId _patternId;

        bool _isPrivate = true;

        public Pattern(PatternId patternId)
        {
            _patternId = patternId;
        }

        public DesignerId DesignerId { get { return PatternId.DesignerId; } }

        public bool IsPrivate { get { return _isPrivate; } set { _isPrivate = value; } }

        public bool IsPublic { get { return !_isPrivate; } }

        public PatternId PatternId { get { return _patternId; } }

        public string Reference { get { return PatternId.Reference; } }
    }
}

namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Pattern
    {
        readonly PatternId _patternId;

        bool _onDisplay = true;

        public Pattern(PatternId patternId)
        {
            _patternId = patternId;
        }

        public DesignerKey DesignerKey { get { return PatternId.DesignerKey; } }

        public bool OnDisplay { get { return _onDisplay; } set { _onDisplay = value; } }

        public PatternId PatternId { get { return _patternId; } }

        public string Reference { get { return PatternId.Reference; } }
    }
}

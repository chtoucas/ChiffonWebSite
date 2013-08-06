namespace Chiffon.Entities
{
    using System;
    using Chiffon.Infrastructure;
    using Narvalo;

    [Serializable]
    public class Pattern
    {
        readonly PatternId _patternId;

        bool _showcased = false;
        bool _preferred = false;
        bool _published = true;

        public Pattern(PatternId patternId)
        {
            _patternId = patternId;
        }

        public DateTime CreationTime { get; set; }

        public DesignerKey DesignerKey { get { return PatternId.DesignerKey; } }

        public bool Locked { get { return Preferred || Showcased; } }

        public PatternId PatternId { get { return _patternId; } }

        public bool Preferred
        {
            get { return _preferred; }
            set
            {
                if (!Published) {
                    throw new InvalidOperationException("You must published the pattern before.");
                }
                _preferred = value;
            }
        }

        public bool Published
        {
            get { return _published; }
            set
            {
                if (Locked) {
                    throw new InvalidOperationException("You must unlocked the pattern before.");
                }
                _published = value;
            }
        }

        public string Reference { get { return PatternId.Reference; } }

        public bool Showcased
        {
            get { return _showcased; }
            set
            {
                if (!Published) {
                    throw new InvalidOperationException("You must published the pattern before.");
                }
                _showcased = value;
            }
        }

        public PatternImage GetImage(PatternSize size)
        {
            return PatternImage.Create(DesignerKey.ToString(), Reference, size);
        }

        public PatternVisibility GetVisibility(PatternSize size)
        {
            if (Published) {
                switch (size) {
                    case PatternSize.Original:
                        return PatternVisibility.Members;
                    case PatternSize.Preview:
                        return (Preferred || Showcased)
                            ? PatternVisibility.Public
                            : PatternVisibility.Members;
                    default:
                        throw ExceptionFactory.NotSupported(
                            "The pattern size '{0}' is not yet handled.", size.ToString());
                }
            }
            else {
                return PatternVisibility.None;
            }
        }
    }
}

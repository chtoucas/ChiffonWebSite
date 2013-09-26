namespace Chiffon.Entities
{
    using System;
    using Chiffon.Infrastructure;
    using Narvalo;

    [Serializable]
    public class PatternView 
    {
        readonly PatternId _patternId;
        readonly string _version;

        bool _showcased = false;
        bool _preferred = false;
        bool _published = true;

        public PatternView(PatternId patternId, string version)
        {
            _patternId = patternId;
            _version = version;
        }

        public string CategoryKey { get; set; }
        public DateTime CreationTime { get; set; }
        public DesignerKey DesignerKey { get { return PatternId.DesignerKey; } }
        public DateTime LastModifiedTime { get; set; }
        public bool Locked { get { return Preferred || Showcased; } }
        public PatternId PatternId { get { return _patternId; } }

        public bool Preferred
        {
            get { return _preferred; }
            set
            {
                if (!Published) {
                    throw new InvalidOperationException("First, you must publish the pattern.");
                }
                _preferred = value;
            }
        }

        public bool Published
        {
            get { return _published; }
            set
            {
                if (Locked && !value) {
                    throw new InvalidOperationException("First, you must unlock the pattern.");
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
                    throw new InvalidOperationException("First, you must publish the pattern.");
                }
                _showcased = value;
            }
        }

        public string Version { get { return _version; } }

        public PatternImage GetImage(PatternSize size)
        {
            return PatternImage.Create(DesignerKey.ToString() + Version, Reference, size);
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
                        throw new NotSupportedException();
                }
            }
            else {
                return PatternVisibility.None;
            }
        }
    }
}

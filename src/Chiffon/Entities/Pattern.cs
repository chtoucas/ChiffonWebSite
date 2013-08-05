namespace Chiffon.Entities
{
    using System;
    using Chiffon.Infrastructure;

    public enum PatternVisibility
    {
        None = 0,
        Members = 1,
        Public = 2,
    }

    public static class PatternExtensions
    {
        //public static bool IsPublic(this Pattern pattern, PatternSize size)
        //{
        //    return size == PatternSize.Preview
        //        && pattern.Published
        //        && (pattern.Preferred || pattern.Showcased);
        //}

        public static PatternVisibility GetVisibility(this Pattern pattern, PatternSize size)
        {
            if (pattern.Published) {
                switch (size) {
                    case PatternSize.Original:
                        return PatternVisibility.Members;
                    case PatternSize.Preview:
                        return (pattern.Preferred || pattern.Showcased)
                            ? PatternVisibility.Public
                            : PatternVisibility.Members;
                    default:
                        throw new InvalidOperationException("XXX");
                }
            }
            else {
                return PatternVisibility.None;
            }
        }
    }

    [Serializable]
    public class Pattern
    {
        readonly PatternId _patternId;

        bool _preferred = false;
        bool _published = true;
        bool _showcased = false;

        public Pattern(PatternId patternId, bool published)
        {
            _patternId = patternId;
            _published = published;
        }

        public DesignerKey DesignerKey { get { return PatternId.DesignerKey; } }

        public PatternId PatternId { get { return _patternId; } }

        public bool Preferred
        {
            get { return _preferred; }
            set
            {
                if (!_published) {
                    throw new InvalidOperationException("XXX");
                }
                _preferred = value;
            }
        }

        public bool Published { get { return _published; } set { _published = value; } }

        public string Reference { get { return PatternId.Reference; } }

        public bool Showcased
        {
            get { return _showcased; }
            set
            {
                if (!_published) {
                    throw new InvalidOperationException("XXX");
                }
                _showcased = value;
            }
        }

        public PatternImage GetImage(PatternSize size)
        {
            return PatternImage.Create(DesignerKey.ToString(), Reference, size);
        }
    }
}

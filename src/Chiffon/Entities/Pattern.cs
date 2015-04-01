namespace Chiffon.Entities
{
    using System;
    using System.Diagnostics.Contracts;

    using Chiffon.Infrastructure;
    using Narvalo;

    public class Pattern
    {
        private readonly PatternId _patternId;
        private readonly string _variant;

        private bool _showcased = false;
        private bool _preferred = false;
        private bool _published = true;

        public Pattern(PatternId patternId, string variant)
        {
            _patternId = patternId;
            _variant = variant;
        }

        public string CategoryKey { get; set; }

        public DateTime CreationTime { get; set; }

        public DesignerKey DesignerKey { get { return PatternId.DesignerKey; } }

        public bool HasPreview { get; set; }

        public DateTime LastModifiedTime { get; set; }

        public bool Locked { get { return Preferred || Showcased; } }

        public PatternId PatternId { get { return _patternId; } }

        public bool Preferred
        {
            get
            {
                return _preferred;
            }

            set
            {
                if (!Published)
                {
                    throw new InvalidOperationException("First, you must publish the pattern.");
                }

                _preferred = value;
            }
        }

        public bool Published
        {
            get
            {
                return _published;
            }

            set
            {
                //if (Locked && !value) {
                //    throw new InvalidOperationException("First, you must unlock the pattern.");
                //}
                _published = value;
            }
        }

        public string Reference { get { return PatternId.Reference; } }

        public string Variant { get { return _variant; } }

        public bool Showcased
        {
            get
            {
                return _showcased;
            }

            set
            {
                if (!Published)
                {
                    throw new InvalidOperationException("First, you must publish the pattern.");
                }

                _showcased = value;
            }
        }

        public PatternImage GetImage(PatternSize size)
        {
            Contract.Ensures(Contract.Result<PatternImage>() != null);

            return PatternImage.Create(DesignerKey.ToString(), CategoryKey.ToString(), Reference, Variant, size);
        }

        public PatternVisibility GetVisibility(PatternSize size)
        {
            if (Published)
            {
                switch (size)
                {
                    case PatternSize.Original:
                        return PatternVisibility.Members;
                    case PatternSize.Preview:
                        return (Preferred || Showcased)
                            ? PatternVisibility.Public
                            : PatternVisibility.Members;
                    default:
                        throw Acknowledge.Unreachable(
                            "The pattern size '" + size.ToString() + "' is not yet handled.");
                }
            }
            else
            {
                return PatternVisibility.None;
            }
        }
    }
}

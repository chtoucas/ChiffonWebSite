namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    public abstract class PatternImage
    {
        public const string MimeType = "image/jpeg";

        protected PatternImage() { }

        public string Directory { get; private set; }
        public string Reference { get; private set; }

        public string RelativePath { get { return Path.Combine(Directory, Filename); } }

        public abstract string Filename { get; }
        public abstract PatternSize Size { get; }

        public static PatternImage Create(string directory, string reference, PatternSize size)
        {
            switch (size) {
                case PatternSize.Preview:
                    return new Preview(size) { Directory = directory, Reference = reference };
                case PatternSize.Original:
                    return new Original { Directory = directory, Reference = reference };
                default:
                    throw new ArgumentException("XXX");
            }
        }

        class Original : PatternImage
        {
            string _filename;

            public Original() : base() { }

            public override string Filename
            {
                get
                {
                    if (_filename == null) {
                        _filename = String.Format(CultureInfo.InvariantCulture, "motif-{0}.jpg", Reference);
                    }
                    return _filename;
                }
            }

            public override PatternSize Size { get { return PatternSize.Original; } }
        }

        class Preview : PatternImage
        {
            static Dictionary<PatternSize, string> SizeNames_ = new Dictionary<PatternSize, string>() {
                { PatternSize.Preview, "apercu"}
            };

            readonly PatternSize _size;

            string _filename;

            public Preview(PatternSize sizeVersion)
                : base()
            {
                _size = sizeVersion;
            }

            public override string Filename
            {
                get
                {
                    if (_filename == null) {
                        _filename = String.Format(CultureInfo.InvariantCulture,
                            "motif-{0}_{1}.jpg", Reference, SizeNames_[Size]);
                    }
                    return _filename;
                }
            }

            public override PatternSize Size { get { return _size; } }
        }
    }
}

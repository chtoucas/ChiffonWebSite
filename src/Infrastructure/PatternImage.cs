namespace Chiffon.Infrastructure
{
    using System;
    using System.Globalization;
    using System.IO;
    using Narvalo;

    public abstract class PatternImage
    {
        const string JpegMimeType_ = "image/jpeg";

        string _relativePath;

        protected PatternImage() { }

        public string Directory { get; private set; }
        public string Reference { get; private set; }

        public string RelativePath
        {
            get
            {
                if (_relativePath == null) {
                    _relativePath = Path.Combine(Directory, Filename);
                }
                return _relativePath;
            }
        }

        public abstract string Filename { get; }
        public abstract string MimeType { get; }
        public abstract PatternSize Size { get; }

        public static PatternImage Create(string directory, string reference, PatternSize size)
        {
            Requires.NotNullOrEmpty(directory, "directory");
            Requires.NotNullOrEmpty(reference, "reference");

            switch (size) {
                case PatternSize.Preview:
                    return new Preview { Directory = directory, Reference = reference };
                case PatternSize.Original:
                    return new Original { Directory = directory, Reference = reference };
                default:
                    throw new InvalidOperationException();
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

            public override string MimeType { get { return JpegMimeType_; } }
            public override PatternSize Size { get { return PatternSize.Original; } }
        }

        class Preview : PatternImage
        {
            string _filename;

            public Preview() : base() { }

            public override string Filename
            {
                get
                {
                    if (_filename == null) {
                        _filename = String.Format(CultureInfo.InvariantCulture, "motif-{0}_apercu.jpg", Reference);
                    }
                    return _filename;
                }
            }

            public override string MimeType { get { return JpegMimeType_; } }
            public override PatternSize Size { get { return PatternSize.Preview; } }
        }
    }
}

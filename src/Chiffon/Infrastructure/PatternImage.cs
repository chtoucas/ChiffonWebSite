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

        public string DesignerDirectory { get; private set; }
        public string CategoryDirectory { get; private set; }
        public string Reference { get; private set; }
        public string Version { get; private set; }

        public string RelativePath
        {
            get
            {
                if (_relativePath == null) {
                    _relativePath = Path.Combine(DesignerDirectory, CategoryDirectory, FileName);
                }
                return _relativePath;
            }
        }

        public abstract string FileName { get; }
        public abstract string MimeType { get; }
        public abstract PatternSize Size { get; }

        public static PatternImage Create(
            string designerDirectory,
            string categoryDirectory,
            string reference,
            string version,
            PatternSize size)
        {
            Require.NotNullOrEmpty(designerDirectory, "directory");
            Require.NotNullOrEmpty(reference, "reference");

            switch (size) {
                case PatternSize.Preview:
                    return new Preview {
                        CategoryDirectory = categoryDirectory,
                        DesignerDirectory = designerDirectory,
                        Reference = reference,
                        Version = version,
                    };
                case PatternSize.Original:
                    return new Original {
                        CategoryDirectory = categoryDirectory,
                        DesignerDirectory = designerDirectory,
                        Reference = reference,
                        Version = version,
                    };
                default:
                    throw new InvalidOperationException();
            }
        }

        class Original : PatternImage
        {
            string _fileName;

            public Original() : base() { }

            public override string FileName
            {
                get
                {
                    if (_fileName == null) {
                        _fileName = String.Format(CultureInfo.InvariantCulture, "motif-{0}{1}.jpg", Reference, Version);
                    }
                    return _fileName;
                }
            }

            public override string MimeType { get { return JpegMimeType_; } }
            public override PatternSize Size { get { return PatternSize.Original; } }
        }

        class Preview : PatternImage
        {
            string _filename;

            public Preview() : base() { }

            public override string FileName
            {
                get
                {
                    if (_filename == null) {
                        _filename = String.Format(CultureInfo.InvariantCulture, "motif-{0}{1}-apercu.jpg", Reference, Version);
                    }
                    return _filename;
                }
            }

            public override string MimeType { get { return JpegMimeType_; } }
            public override PatternSize Size { get { return PatternSize.Preview; } }
        }
    }
}

namespace Chiffon.Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.IO;

    using Narvalo;

    public abstract class PatternImage
    {
        private const string JPEG_MIME_TYPE = "image/jpeg";

        private string _relativePath;

        protected PatternImage() { }

        public string DesignerDirectory { get; private set; }

        public string CategoryDirectory { get; private set; }

        public string Reference { get; private set; }

        public string Version { get; private set; }

        public string RelativePath
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                if (_relativePath == null)
                {
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
            Contract.Ensures(Contract.Result<PatternImage>() != null);

            switch (size)
            {
                case PatternSize.Preview:
                    return new Preview_ {
                        CategoryDirectory = categoryDirectory,
                        DesignerDirectory = designerDirectory,
                        Reference = reference,
                        Version = version,
                    };
                case PatternSize.Original:
                    return new Original_ {
                        CategoryDirectory = categoryDirectory,
                        DesignerDirectory = designerDirectory,
                        Reference = reference,
                        Version = version,
                    };
                default:
                    throw Acknowledge.Unreachable(
                        "The pattern size '" + size.ToString() + "' is not yet handled.");
            }
        }

        private sealed class Original_ : PatternImage
        {
            private string _fileName;

            public Original_() : base() { }

            public override string FileName
            {
                get
                {
                    if (_fileName == null)
                    {
                        _fileName = "motif-" + Reference + Version + ".jpg";
                    }

                    return _fileName;
                }
            }

            public override string MimeType { get { return JPEG_MIME_TYPE; } }

            public override PatternSize Size { get { return PatternSize.Original; } }
        }

        private sealed class Preview_ : PatternImage
        {
            private string _filename;

            public Preview_() : base() { }

            public override string FileName
            {
                get
                {
                    if (_filename == null)
                    {
                        _filename = "motif-" + Reference + Version + "-apercu.jpg";
                    }

                    return _filename;
                }
            }

            public override string MimeType { get { return JPEG_MIME_TYPE; } }

            public override PatternSize Size { get { return PatternSize.Preview; } }
        }
    }
}

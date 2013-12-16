namespace Narvalo.Web.Semantic
{
    using System;
    using Narvalo;

    public class OpenGraphImage
    {
        public const string JpegMimeType = "image/jpeg";
        public const string PngMimeType = "image/png";

        Uri _url;

        public Uri Url
        {
            get { return _url; }
            set { Requires.NotNull(value, "value"); _url = value; }
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public string MimeType { get; set; }
    }
}

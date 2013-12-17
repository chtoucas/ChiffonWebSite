namespace Narvalo.Web.Semantic
{
    using System;
    using Narvalo;

    public class OpenGraphImage
    {
        public const string JpegMimeType = "image/jpeg";
        public const string PngMimeType = "image/png";

        readonly Uri _url;

        public OpenGraphImage(Uri url)
        {
            Requires.NotNull(url, "url");

            _url = url;
        }

        public Uri Url { get { return _url; } }

        public int Height { get; set; }
        public int Width { get; set; }
        public string MimeType { get; set; }
    }
}

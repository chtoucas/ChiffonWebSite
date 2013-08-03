namespace Chiffon.Infrastructure
{
    public class PatternFile
    {
        public const string MimeType = "image/jpeg";

        public string Directory { get; set; }
        public bool IsPublic { get; set; }
        public string Reference { get; set; }
    }
}

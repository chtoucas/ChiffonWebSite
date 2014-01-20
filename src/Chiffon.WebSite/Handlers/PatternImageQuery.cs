namespace Chiffon.Handlers
{
    using Chiffon.Entities;
    using Chiffon.Infrastructure;

    public class PatternImageQuery
    {
        public DesignerKey DesignerKey { get; set; }
        public string Reference { get; set; }
        public PatternSize Size { get; set; }
        public string Variant { get; set; }
    }
}
namespace Chiffon.Handlers
{
    using System.ComponentModel.DataAnnotations;

    using Chiffon.Entities;
    using Chiffon.Infrastructure;

    public class PatternImageQuery
    {
        [Required]
        public DesignerKey DesignerKey { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Reference { get; set; }

        [Required]
        public PatternSize Size { get; set; }

        public string Variant { get; set; }
    }
}
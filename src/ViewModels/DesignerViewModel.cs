namespace Chiffon.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Entities;

    public class DesignerViewModel
    {
        public DesignerKey DesignerKey { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string Presentation { get; set; }
        public IEnumerable<CategoryItem> Categories { get; set; }
        public IEnumerable<PatternItem> Previews { get; set; }

        public int PatternCount
        {
            get
            {
                return (from _ in Categories select _.PatternCount).Sum();
            }
        }
    }
}
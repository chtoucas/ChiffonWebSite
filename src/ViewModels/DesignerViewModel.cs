namespace Chiffon.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class DesignerViewModel
    {
        public DesignerItem Designer { get; set; }
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
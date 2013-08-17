namespace Chiffon.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class CategoryViewModel
    {
        public DesignerViewItem Designer { get; set; }
        public IEnumerable<CategoryViewItem> Categories { get; set; }
        public IEnumerable<PatternViewItem> Patterns { get; set; }

        public int PatternCount
        {
            get
            {
                return (from _ in Categories select _.PatternCount).Sum();
            }
        }
    }
}
namespace Chiffon.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class DesignerViewModel
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
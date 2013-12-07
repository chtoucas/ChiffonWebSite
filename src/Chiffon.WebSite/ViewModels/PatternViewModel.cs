namespace Chiffon.ViewModels
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class PatternViewModel
    {
        public CategoryViewItem Category { get; set; }
        public DesignerViewItem Designer { get; set; }
        public string Reference { get; set; }

        public IEnumerable<PatternViewItem> PatternViews { get; set; }
        //public IEnumerable<PatternViewItem> Previews { get; set; }

        //public int PageCount { get; set; }
        //public int PageIndex { get; set; }
        //public bool IsFirstPage { get; set; }
        //public bool IsLastPage { get; set; }
    }
}
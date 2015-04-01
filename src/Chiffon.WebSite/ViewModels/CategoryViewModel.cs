namespace Chiffon.ViewModels
{
    using System.Collections.Generic;

    public sealed class CategoryViewModel
    {
        public CategoryViewItem Category { get; set; }

        public DesignerViewItem Designer { get; set; }

        public IEnumerable<PatternViewItem> Previews { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }
    }
}
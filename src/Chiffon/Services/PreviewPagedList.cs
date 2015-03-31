namespace Chiffon.Services
{
    using System.Collections.Generic;

    using Chiffon.Entities;

    public class PreviewPagedList
    {
        public bool IsFirstPage { get { return PageIndex == 1; } }

        public bool IsLastPage { get { return PageIndex == PageCount; } }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public IEnumerable<Pattern> Previews { get; set; }
    }
}
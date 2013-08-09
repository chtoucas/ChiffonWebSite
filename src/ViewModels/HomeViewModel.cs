namespace Chiffon.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class HomeViewModel
    {
        public IList<ShowcasedPattern> Patterns { get; set; }

        public class ShowcasedPattern
        {
            public Uri Url { get; set; }
            public Uri PreviewImageUrl { get; set; }
        }
    }
}

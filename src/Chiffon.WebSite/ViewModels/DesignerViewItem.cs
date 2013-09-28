﻿namespace Chiffon.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Entities;

    [Serializable]
    public class DesignerViewItem
    {
        int? _patternCount;

        public IEnumerable<CategoryViewItem> Categories { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public DesignerKey Key { get; set; }
        public string Presentation { get; set; }

        public int PatternCount
        {
            get
            {
                if (!_patternCount.HasValue) {
                    _patternCount = (from _ in Categories select _.PatternsCount).Sum();
                }
                return _patternCount.Value;
            }
        }
    }
}
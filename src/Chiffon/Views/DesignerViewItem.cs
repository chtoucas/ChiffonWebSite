namespace Chiffon.Views
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Chiffon.Entities;
    using Narvalo;

    public sealed class DesignerViewItem
    {
        private int? _patternCount;

        public IEnumerable<CategoryViewItem> Categories { get; set; }

        public string CurrentCategoryKey { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public DesignerKey Key { get; set; }

        public string Presentation { get; set; }

        public int PatternCount
        {
            get
            {
                if (!_patternCount.HasValue)
                {
                    _patternCount = (from _ in Categories select _.PatternsCount).Sum();
                }

                return _patternCount.Value;
            }
        }

        public static DesignerViewItem Of(Designer designer, IEnumerable<Category> categories, string categoryKey)
        {
            Require.NotNull(designer, "designer");
            Contract.Requires(categories != null);

            return new DesignerViewItem {
                Categories = from _ in categories select CategoryViewItem.Of(_),
                CurrentCategoryKey = categoryKey,
                DisplayName = designer.DisplayName,
                Email = designer.Email,
                Key = designer.Key,
                Presentation = designer.Presentation,
            };
        }
    }
}
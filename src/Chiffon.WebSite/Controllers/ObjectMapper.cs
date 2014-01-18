namespace Chiffon.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo;

    public static class ObjectMapper
    {
        public static CategoryViewItem Map(Category category)
        {
            Require.NotNull(category, "category");

            return new CategoryViewItem {
                DisplayName = category.DisplayName,
                Key = category.Key,
                PatternsCount = category.PatternsCount,
            };
        }

        public static DesignerViewItem Map(Designer designer, IEnumerable<Category> categories, string categoryKey)
        {
            Require.NotNull(designer, "designer");

            return new DesignerViewItem {
                Categories = from _ in categories select Map(_),
                CurrentCategoryKey = categoryKey,
                DisplayName = designer.DisplayName,
                Email = designer.Email,
                Key = designer.Key,
                Presentation = designer.Presentation,
            };
        }

        public static PatternViewItem Map(Pattern pattern, string designerName)
        {
            Require.NotNull(pattern, "pattern");

            return new PatternViewItem {
                CategoryKey = pattern.CategoryKey,
                DesignerKey = pattern.DesignerKey,
                DesignerName = designerName,
                Reference = pattern.Reference,
                Variant = pattern.Variant,
            };
        }
    }
}

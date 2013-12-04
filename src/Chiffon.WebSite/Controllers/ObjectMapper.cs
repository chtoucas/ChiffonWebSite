﻿namespace Chiffon.Controllers
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
            Requires.NotNull(category, "category");

            return new CategoryViewItem {
                DisplayName = category.DisplayName,
                Key = category.Key,
                PatternsCount = category.PatternsCount,
            };
        }

        public static DesignerViewItem Map(Designer designer, IEnumerable<Category> categories)
        {
            Requires.NotNull(designer, "designer");

            return new DesignerViewItem {
                Categories = from _ in categories select Map(_),
                DisplayName = designer.DisplayName,
                EmailAddress = designer.EmailAddress.ToString(),
                Key = designer.Key,
                Presentation = designer.Presentation,
            };
        }

        public static PatternViewItem Map(Pattern pattern, string designerName)
        {
            Requires.NotNull(pattern, "pattern");

            return new PatternViewItem {
                CategoryKey = pattern.CategoryKey,
                DesignerKey = pattern.DesignerKey,
                DesignerName = designerName,
                Reference = pattern.Reference,
                Version = pattern.Version,
            };
        }
    }
}

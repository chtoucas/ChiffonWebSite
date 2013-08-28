namespace Chiffon.Controllers
{
    using Chiffon.Entities;
    using Chiffon.ViewModels;

    public static class Mapper
    {
        public static CategoryViewItem Map(Category category)
        {
            return new CategoryViewItem {
                DisplayName = category.DisplayName,
                Key = category.Key,
                PatternsCount = category.PatternsCount,
            };
        }

        public static DesignerViewItem Map(Designer designer)
        {
            return new DesignerViewItem {
                DisplayName = designer.DisplayName,
                EmailAddress = designer.EmailAddress.ToString(),
                Key = designer.Key,
                Presentation = designer.Presentation,
            };
        }

        public static PatternViewItem Map(Pattern pattern, string designerName)
        {
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

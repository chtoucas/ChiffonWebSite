namespace Chiffon.Data
{
    using Chiffon.Entities;
    using Chiffon.ViewModels;

    public static class Mapper
    {
        public static PatternViewItem Map(Pattern pattern, string designerName)
        {
            return new PatternViewItem {
                CategoryKey = pattern.CategoryKey,
                DesignerKey = pattern.DesignerKey,
                DesignerName = designerName,
                Reference = pattern.Reference,
            };
        }
    }
}

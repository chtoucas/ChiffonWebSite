namespace Chiffon.Views
{
    using Chiffon.Entities;
    using Narvalo;

    public sealed class CategoryViewItem
    {
        public string DisplayName { get; set; }

        public string Key { get; set; }

        public int PatternsCount { get; set; }

        public static CategoryViewItem Of(Category category)
        {
            Require.NotNull(category, "category");

            return new CategoryViewItem {
                DisplayName = category.DisplayName,
                Key = category.Key,
                PatternsCount = category.PatternsCount,
            };
        }
    }
}
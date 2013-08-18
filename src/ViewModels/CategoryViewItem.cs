namespace Chiffon.ViewModels
{
    using System;

    [Serializable]
    public class CategoryViewItem
    {
        public string DisplayName { get; set; }
        public string Key { get; set; }
        public int PatternsCount { get; set; }
    }
}
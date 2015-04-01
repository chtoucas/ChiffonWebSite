namespace Chiffon.Entities
{
    using Narvalo;

    public sealed class Category
    {
        private readonly DesignerKey _designerKey;
        private readonly string _key;

        public Category(DesignerKey designerKey, string key)
        {
            Require.NotNullOrEmpty(key, "key");

            _designerKey = designerKey;
            _key = key;
        }

        public DesignerKey DesignerKey { get { return _designerKey; } }

        public string DisplayName { get; set; }

        public string Key { get { return _key; } }

        public int PatternsCount { get; set; }
    }
}

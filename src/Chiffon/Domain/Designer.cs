namespace Chiffon.Domain
{
    using System;

    [Serializable]
    public class Designer
    {
        public DesignerId DesignerId { get; set; }

        public string DisplayName { get; set; }

        public string PatternDirectory { get; set; }

        public string UrlKey { get; set; }
    }
}

namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public class Member
    {
        public string DisplayName { get; set; }
        public MemberId MemberId { get; set; }
        public string PatternDirectory { get; set; }
        public string UrlKey { get; set; }
    }
}

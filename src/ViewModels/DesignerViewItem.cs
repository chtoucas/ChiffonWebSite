namespace Chiffon.ViewModels
{
    using System;
    using Chiffon.Entities;

    [Serializable]
    public class DesignerViewItem
    {
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public DesignerKey Key { get; set; }
        public string Presentation { get; set; }
    }
}
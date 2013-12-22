namespace Chiffon.ViewModels
{
    using System;
    using System.Globalization;
    using System.Web;
    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Resources;

    public class PatternViewItem
    {
        public string CategoryKey { get; set; }
        public DesignerKey DesignerKey { get; set; }
        public string DesignerName { get; set; }
        public string Reference { get; set; }
        public string Variant { get; set; }

        public string CssClass { get { return CssUtility.DesignerClass(DesignerKey); } }

        public IHtmlString Description
        {
            get
            {
                return new HtmlString(String.Format(CultureInfo.CurrentCulture,
                    SR.PatternDescriptionFormat, Reference, DesignerName));
            }
        }

    }
}

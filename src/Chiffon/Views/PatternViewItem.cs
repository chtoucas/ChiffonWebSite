namespace Chiffon.Views
{
    using System;
    using System.Web;

    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Properties;
    using Narvalo;

    public sealed class PatternViewItem
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
#if SHOWCASE
                return DesignerKey == DesignerKey.VivianeDevaux
                    ? new HtmlString(Format.CurrentCulture(Strings.PatternDescriptionFormat, Reference, DesignerName))
                    : new HtmlString(String.Empty);
#else
                return new HtmlString(Format.CurrentCulture(Strings.PatternDescriptionFormat, Reference, DesignerName));
#endif
            }
        }
    }
}

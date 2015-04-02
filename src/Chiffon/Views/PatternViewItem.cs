namespace Chiffon.Views
{
    using System;
    using System.Web;

    using Chiffon.Entities;
    using Narvalo;

    public sealed class PatternViewItem
    {
        public string CategoryKey { get; set; }

        public DesignerKey DesignerKey { get; set; }

        public string DesignerName { get; set; }

        public string Reference { get; set; }

        public string Variant { get; set; }

        public string CssClass { get { return ViewUtility.DesignerClass(DesignerKey); } }

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

        public static PatternViewItem Of(Pattern pattern, string designerName)
        {
            Require.NotNull(pattern, "pattern");

            return new PatternViewItem {
                CategoryKey = pattern.CategoryKey,
                DesignerKey = pattern.DesignerKey,
                DesignerName = designerName,
                Reference = pattern.Reference,
                Variant = pattern.Variant,
            };
        }
    }
}

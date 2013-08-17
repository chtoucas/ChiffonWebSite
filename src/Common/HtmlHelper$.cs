namespace Chiffon.Common
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using Chiffon.Resources;
    using Chiffon.ViewModels;

    public static class HtmlHelperExtensions
    {
        public IHtmlString PatternDescription(this HtmlHelper self, PatternViewItem pattern, string designerName)
        {
            return new HtmlString(String.Format(CultureInfo.CurrentCulture,
                SR.PatternDescriptionFormat, pattern.Reference, designerName));
        }
    }
}
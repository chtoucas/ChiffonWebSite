namespace Chiffon.Common
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class HtmlHelperExtensions
    {
        //public IHtmlString PatternDescription(this HtmlHelper self, PatternViewItem pattern, string designerName)
        //{
        //    return new HtmlString(String.Format(CultureInfo.CurrentCulture,
        //        SR.PatternDescriptionFormat, pattern.Reference, designerName));
        //}

        // FIXME: Utiliser ChiffonLanguage. Cf. aussi le HomeController.About()
        public static IHtmlString PageTitle(this HtmlHelper @this, string languageName)
        {
            if (languageName == "en") {
                return @this.Partial(ViewName.Shared.PageTitleEnglish);
            }
            else {
                return @this.Partial(ViewName.Shared.PageTitle);
            }
        }
    }
}
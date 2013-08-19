namespace Chiffon.Common
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Chiffon.Infrastructure;

    public static class HtmlHelperExtensions
    {
        //public IHtmlString PatternDescription(this HtmlHelper self, PatternViewItem pattern, string designerName)
        //{
        //    return new HtmlString(String.Format(CultureInfo.CurrentCulture,
        //        SR.PatternDescriptionFormat, pattern.Reference, designerName));
        //}

        public static IHtmlString PageTitle(this HtmlHelper @this, ChiffonLanguage language)
        {
            switch (language) {
                case ChiffonLanguage.English:
                    return @this.Partial(ViewName.Shared.PageTitleEnglish);
                case ChiffonLanguage.Default:
                default:
                    return @this.Partial(ViewName.Shared.PageTitle);
            }
        }
    }
}
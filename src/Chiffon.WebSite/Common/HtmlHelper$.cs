namespace Chiffon.Common
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Chiffon.Infrastructure;

    public static class HtmlHelperExtensions
    {
        public static IHtmlString ContactTitle(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Shared.ContactTitle, language));
        }

        public static IHtmlString PageTitle(this HtmlHelper @this, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(ViewName.Shared.PageTitle, language));
        }
    }
}
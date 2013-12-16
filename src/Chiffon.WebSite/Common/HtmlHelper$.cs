namespace Chiffon.Common
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Chiffon.Infrastructure;

    public static class HtmlHelperExtensions
    {
        public static IHtmlString Partial(this HtmlHelper @this, string viewName, ChiffonLanguage language)
        {
            return @this.Partial(ViewUtility.Localize(viewName, language));
        }
    }
}
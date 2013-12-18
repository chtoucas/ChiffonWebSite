namespace Chiffon.Common
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Chiffon.Infrastructure;

    public static class HtmlHelperExtensions
    {
        // TODO: Utiliser des ChildAction avec cache si c'est possible.
        public static void RenderPartial(this HtmlHelper @this, string viewName, ChiffonLanguage language)
        {
            @this.RenderPartial(ViewUtility.Localize(viewName, language));
        }

        public static void RenderWidget(this HtmlHelper @this, string viewName)
        {
            @this.RenderAction(viewName, Constants.ControllerName.Widget);
        }

        public static void RenderWidget(this HtmlHelper @this, string viewName, object routeValues)
        {
            @this.RenderAction(viewName, Constants.ControllerName.Widget, routeValues);
        }

        public static void RenderWidget(this HtmlHelper @this, string viewName, RouteValueDictionary routeValues)
        {
            @this.RenderAction(viewName, Constants.ControllerName.Widget, routeValues);
        }
    }
}
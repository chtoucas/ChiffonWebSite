namespace Chiffon.WebSite.Common
{
    using System.Web.Routing;
    using System.Web.Mvc;

    public static class UrlHelperExtensions
    {
        public static string SecureAction(this UrlHelper helper, string actionName, string controllerName, object routeValues)
        {
            return helper.SecureAction(actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        public static string SecureAction(this UrlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            if (helper.RequestContext.HttpContext.User.Identity.IsAuthenticated) {
                return helper.Action(actionName, controllerName, routeValues);
            }
            else {
                return helper.Action("Register", "Account");
            }
        }
    }
}

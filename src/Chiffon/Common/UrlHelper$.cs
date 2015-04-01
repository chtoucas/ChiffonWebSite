namespace Chiffon.Common
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Web;

    public static class UrlHelperExtensions
    {
        #region Designer controller.

        public static string Designer(this UrlHelper @this, DesignerKey designerKey, int pageIndex)
        {
            Acknowledge.Object(@this);

            object routeValues;
            if (pageIndex > 1)
            {
                routeValues = new { designerKey = designerKey.ToString(), p = pageIndex };
            }
            else
            {
                routeValues = new { designerKey = designerKey.ToString() };
            }

            return SecureAction(@this, Constants.ActionName.Designer.Index, Constants.ControllerName.Designer, routeValues);
        }

        public static string Category(this UrlHelper @this, DesignerKey designerKey, string categoryKey, int pageIndex)
        {
            Acknowledge.Object(@this);

            object routeValues;
            if (pageIndex > 1)
            {
                routeValues = new { designerKey = designerKey.ToString(), categoryKey = categoryKey, p = pageIndex };
            }
            else
            {
                routeValues = new { designerKey = designerKey.ToString(), categoryKey = categoryKey };
            }

            return SecureAction(@this, Constants.ActionName.Designer.Category, Constants.ControllerName.Designer, routeValues);
        }

        public static string Pattern(this UrlHelper @this, DesignerKey designerKey, string categoryKey, string reference, int pageIndex)
        {
            Acknowledge.Object(@this);

            object routeValues;
            if (pageIndex > 1)
            {
                routeValues = new { designerKey = designerKey.ToString(), categoryKey = categoryKey, reference = reference, p = pageIndex };
            }
            else
            {
                routeValues = new { designerKey = designerKey.ToString(), categoryKey = categoryKey, reference = reference };
            }

            return SecureAction(@this, Constants.ActionName.Designer.Pattern, Constants.ControllerName.Designer, routeValues);
        }

        #endregion

        public static string PreviewContent(this UrlHelper @this, DesignerKey designerKey, string reference, string variant)
        {
            Require.Object(@this);

            return PreviewContent_(@this, designerKey, reference, variant, false /* absolute */);
        }

        public static string PreviewContent(this UrlHelper @this, DesignerKey designerKey, string reference, string variant, bool absolute)
        {
            Require.Object(@this);

            return PreviewContent_(@this, designerKey, reference, variant, absolute);
        }

        public static string PatternContent(this UrlHelper @this, DesignerKey designerKey, string reference, string variant)
        {
            Require.Object(@this);

            return @this.Content(
                "~/" + designerKey.ToString() + "/motif-" + reference + "-" + variant + ".jpg");
        }

        public static string SecureAction(this UrlHelper @this, string actionName, string controllerName, object routeValues)
        {
            Require.Object(@this);

            return @this.SecureAction(actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        public static string SecureAction(this UrlHelper @this, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            Require.Object(@this);

            var originalUrl = @this.Action(actionName, controllerName, routeValues);

            if (@this.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return originalUrl;
            }
            else
            {
                return @this.RouteUrl(Constants.RouteName.Account.Register, new { returnUrl = originalUrl });
            }
        }

        public static string SecureUrl(this UrlHelper @this, string routeName, object routeValues)
        {
            Require.Object(@this);

            return @this.SecureUrl(routeName, new RouteValueDictionary(routeValues));
        }

        public static string SecureUrl(this UrlHelper @this, string routeName, RouteValueDictionary routeValues)
        {
            Require.Object(@this);

            var originalUrl = @this.RouteUrl(routeName, routeValues);

            if (@this.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return originalUrl;
            }
            else
            {
                return @this.RouteUrl(Constants.RouteName.Account.Register, new { returnUrl = originalUrl });
            }
        }

        // TODO: Quand on passera à un serveur de media séparé il faudra changer ces utilitaires.
        private static string PreviewContent_(UrlHelper urlHelper, DesignerKey designerKey, string reference, string variant, bool absolute)
        {
            Require.NotNull(urlHelper, "urlHelper");

            var path = "~/" + designerKey.ToString() + "/vignette-" + reference + "-" + variant + ".jpg";

            return absolute ? urlHelper.AbsoluteContent(path) : urlHelper.Content(path);
        }
    }
}

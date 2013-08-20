namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Chiffon.Common;
    using Chiffon.Entities;

    public static class UrlHelperExtensions
    {
        #region Designer controller.

        public static string Designer(this UrlHelper @this, DesignerKey designerKey)
        {
            return SecureAction(@this, ActionName.Designer.Index, ControllerName.Designer, new { designerKey = designerKey });
        }

        public static string Category(this UrlHelper @this, DesignerKey designerKey, string categoryKey)
        {
            return SecureAction(@this, ActionName.Designer.Category, ControllerName.Designer,
                new { designerKey = designerKey, categoryKey = categoryKey });
        }

        public static string Pattern(this UrlHelper @this, DesignerKey designerKey, string categoryKey, string reference)
        {
            return SecureAction(@this, ActionName.Designer.Pattern, ControllerName.Designer,
                new { designerKey = designerKey, categoryKey = categoryKey, reference = reference });
        }

        public static string EstherMarthiUrl(this UrlHelper @this)
        {
            return SecureUrl(@this, RouteName.EstherMarthi.Index, null /* routeValues */);
        }

        public static string VivianeDevauxUrl(this UrlHelper @this)
        {
            return SecureUrl(@this, RouteName.VivianeDevaux.Index, null /* routeValues */);
        }

        public static string ChristineLégeretUrl(this UrlHelper @this)
        {
            return SecureUrl(@this, RouteName.ChristineLégeret.Index, null /* routeValues */);
        }

        public static string LaureRousselUrl(this UrlHelper @this)
        {
            return SecureUrl(@this, RouteName.LaureRoussel.Index, null /* routeValues */);
        }

        #endregion

        public static string PreviewContent(this UrlHelper @this, DesignerKey designerKey, string reference, string version)
        {
            return @this.Content(String.Format("~/{0}/vignette-{1}-{2}.jpg", designerKey, reference, version));
        }

        public static string PatternContent(this UrlHelper @this, DesignerKey designerKey, string reference, string version)
        {
            return @this.Content(String.Format("~/{0}/motif-{1}-{2}.jpg", designerKey, reference, version));
        }

        public static string SecureAction(this UrlHelper @this, string actionName, string controllerName, object routeValues)
        {
            return @this.SecureAction(actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        public static string SecureAction(this UrlHelper @this, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var originalUrl = @this.Action(actionName, controllerName, routeValues);

            if (@this.RequestContext.HttpContext.User.Identity.IsAuthenticated) {
                return originalUrl;
            }
            else {
                return @this.RouteUrl(RouteName.Account.Register, new { returnUrl = originalUrl });
            }
        }

        public static string SecureUrl(this UrlHelper @this, string routeName, object routeValues)
        {
            return @this.SecureUrl(routeName, new RouteValueDictionary(routeValues));
        }

        public static string SecureUrl(this UrlHelper @this, string routeName, RouteValueDictionary routeValues)
        {
            var originalUrl = @this.RouteUrl(routeName, routeValues);

            if (@this.RequestContext.HttpContext.User.Identity.IsAuthenticated) {
                return originalUrl;
            }
            else {
                return @this.RouteUrl(RouteName.Account.Register, new { returnUrl = originalUrl });
            }
        }

        public static string Current(this UrlHelper @this)
        {
            return @this.RequestContext.HttpContext.Request.RawUrl;
        }

        public static string AbsoluteAction(this UrlHelper @this, string actionName, string controllerName, object routeValues)
        {
            var scheme = @this.RequestContext.HttpContext.Request.Url.Scheme;
            return @this.Action(actionName, controllerName, routeValues, scheme);
        }

        public static string AbsoluteAction(this UrlHelper @this, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var scheme = @this.RequestContext.HttpContext.Request.Url.Scheme;
            return @this.Action(actionName, controllerName, routeValues, scheme);
        }

        //public static string AbsoluteContent(this UrlHelper self, string path)
        //{
        //    Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);

        //    //If the URI is not already absolute, rebuild it based on the current request.
        //    if (!uri.IsAbsoluteUri) {
        //        Uri requestUrl = self.RequestContext.HttpContext.Request.Url;
        //        UriBuilder builder = new UriBuilder(requestUrl.Scheme, requestUrl.Host, requestUrl.Port);

        //        builder.Path = VirtualPathUtility.ToAbsolute(path);
        //        uri = builder.Uri;
        //    }

        //    return uri.ToString();
        //}
    }
}

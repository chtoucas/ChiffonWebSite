﻿namespace Chiffon.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
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
            object routeValues;
            if (pageIndex > 1) {
                routeValues = new { designerKey = designerKey.ToString(), p = pageIndex };
            }
            else {
                routeValues = new { designerKey = designerKey.ToString() };
            }

            return SecureAction(@this, Constants.ActionName.Designer.Index, Constants.ControllerName.Designer, routeValues);
        }

        public static string Category(this UrlHelper @this, DesignerKey designerKey, string categoryKey, int pageIndex)
        {
            object routeValues;
            if (pageIndex > 1) {
                routeValues = new { designerKey = designerKey.ToString(), categoryKey = categoryKey, p = pageIndex };
            }
            else {
                routeValues = new { designerKey = designerKey.ToString(), categoryKey = categoryKey };
            }

            return SecureAction(@this, Constants.ActionName.Designer.Category, Constants.ControllerName.Designer, routeValues);
        }

        public static string Pattern(this UrlHelper @this, DesignerKey designerKey, string categoryKey, string reference, int pageIndex)
        {
            object routeValues;
            if (pageIndex > 1) {
                routeValues = new { designerKey = designerKey.ToString(), categoryKey = categoryKey, reference = reference, p = pageIndex };
            }
            else {
                routeValues = new { designerKey = designerKey.ToString(), categoryKey = categoryKey, reference = reference };
            }

            return SecureAction(@this, Constants.ActionName.Designer.Pattern, Constants.ControllerName.Designer, routeValues);
        }

        #endregion

        public static string PreviewContent(this UrlHelper @this, DesignerKey designerKey, string reference, string variant)
        {
            Requires.NotNull(@this, "this");

            return PreviewContent_(@this, designerKey, reference, variant, false /* absolute */);
        }

        public static string PreviewContent(this UrlHelper @this, DesignerKey designerKey, string reference, string variant, bool absolute)
        {
            Requires.NotNull(@this, "this");

            return PreviewContent_(@this, designerKey, reference, variant, absolute);
        }

        public static string PatternContent(this UrlHelper @this, DesignerKey designerKey, string reference, string variant)
        {
            Requires.NotNull(@this, "this");

            return @this.Content(String.Format(CultureInfo.InvariantCulture,
                "~/{0}/motif-{1}-{2}.jpg", designerKey, reference, variant));
        }

        public static string SecureAction(this UrlHelper @this, string actionName, string controllerName, object routeValues)
        {
            Requires.NotNull(@this, "this");

            return @this.SecureAction(actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        public static string SecureAction(this UrlHelper @this, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            Requires.NotNull(@this, "this");

            var originalUrl = @this.Action(actionName, controllerName, routeValues);

            if (@this.RequestContext.HttpContext.User.Identity.IsAuthenticated) {
                return originalUrl;
            }
            else {
                return @this.RouteUrl(Constants.RouteName.Account.Register, new { returnUrl = originalUrl });
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        public static string SecureUrl(this UrlHelper @this, string routeName, object routeValues)
        {
            Requires.NotNull(@this, "this");

            return @this.SecureUrl(routeName, new RouteValueDictionary(routeValues));
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        public static string SecureUrl(this UrlHelper @this, string routeName, RouteValueDictionary routeValues)
        {
            Requires.NotNull(@this, "this");

            var originalUrl = @this.RouteUrl(routeName, routeValues);

            if (@this.RequestContext.HttpContext.User.Identity.IsAuthenticated) {
                return originalUrl;
            }
            else {
                return @this.RouteUrl(Constants.RouteName.Account.Register, new { returnUrl = originalUrl });
            }
        }

        // TODO: Quand on passera à un serveur de media séparé il faudra changer ces utilitaires.
        static string PreviewContent_(UrlHelper urlHelper, DesignerKey designerKey, string reference, string variant, bool absolute)
        {
            Requires.NotNull(urlHelper, "urlHelper");

            var path = String.Format(CultureInfo.InvariantCulture,
                "~/{0}/vignette-{1}-{2}.jpg", designerKey, reference, variant);

            return absolute ? urlHelper.AbsoluteContent(path) : urlHelper.Content(path);
        }
    }
}

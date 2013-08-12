namespace Chiffon.Common
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Chiffon.Entities;

    public static class UrlHelperExtensions
    {
        public static string Designer(this UrlHelper self, DesignerKey designerKey)
        {
            return SecureAction(self, MVC.Designer.ActionNames.Index, MVC.Designer.Name, new { designer = designerKey.ToString() });
        }

        public static string Pattern(this UrlHelper self, DesignerKey designerKey, string reference)
        {
            return SecureAction(self, MVC.Designer.ActionNames.Pattern, MVC.Designer.Name, new { designer = designerKey.ToString(), reference = reference });
        }

        public static string PatternPreview(this UrlHelper self, DesignerKey designerKey, string reference)
        {
            return self.Content(String.Format("~/{0}/motif-{1}_preview.jpg", designerKey, reference));
        }

        public static string OriginalPattern(this UrlHelper self, DesignerKey designerKey, string reference)
        {
            return self.Content(String.Format("~/{0}/motif-{1}.jpg", designerKey, reference));
        }


        public static string SecureAction(this UrlHelper self, string actionName, string controllerName, object routeValues)
        {
            return self.SecureAction(actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        public static string SecureAction(this UrlHelper self, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var originalUrl = self.Action(actionName, controllerName, routeValues);

            if (self.RequestContext.HttpContext.User.Identity.IsAuthenticated) {
                return originalUrl;
            }
            else {
                //return self.Action(MVC.Account.Register(returnUrl: originalUrl));
                return self.Action(MVC.Account.ActionNames.Register, MVC.Account.Name, new { returnUrl = originalUrl });
            }
        }

        public static string Current(this UrlHelper self)
        {
            return self.RequestContext.HttpContext.Request.RawUrl;
        }

        public static string AbsoluteAction(this UrlHelper self, string actionName, string controllerName, object routeValues)
        {
            var scheme = self.RequestContext.HttpContext.Request.Url.Scheme;
            return self.Action(actionName, controllerName, routeValues, scheme);
        }

        public static string AbsoluteAction(this UrlHelper self, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var scheme = self.RequestContext.HttpContext.Request.Url.Scheme;
            return self.Action(actionName, controllerName, routeValues, scheme);
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

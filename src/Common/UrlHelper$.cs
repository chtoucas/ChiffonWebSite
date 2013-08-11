namespace Chiffon.Common
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Chiffon.Entities;

    public static class UrlHelperExtensions
    {
        public static string Member(this UrlHelper self, DesignerKey designerKey)
        {
            return SecureAction(self, "Index", MVC.Member.Name, new { designer = designerKey.ToString() });
        }

        public static string Pattern(this UrlHelper self, DesignerKey designerKey, string reference)
        {
            return SecureAction(self, "Pattern", MVC.Member.Name, new { designer = designerKey.ToString(), reference = reference });
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
            if (self.RequestContext.HttpContext.User.Identity.IsAuthenticated) {
                return self.Action(actionName, controllerName, routeValues);
            }
            else {
                return self.Action(MVC.Account.Register());
            }
        }

        //public static string AbsoluteAction(this UrlHelper self, string actionName, string controllerName, object routeValues)
        //{
        //    return self.Action(actionName, controllerName, routeValues, self.RequestContext.HttpContext.Request.Url.Scheme);
        //}

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

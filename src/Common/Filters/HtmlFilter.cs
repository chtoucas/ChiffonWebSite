namespace Chiffon.Common.Filters
{
    using System;
    using System.Threading;
    using System.Web.Mvc;

    public sealed class HtmlFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["BodyId"] = GetBodyId_(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewData = filterContext.Controller.ViewData;
            var identity = filterContext.RequestContext.HttpContext.User.Identity;

            viewData["Language"] = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName; ;
            viewData["RegisterLink"] = identity.IsAuthenticated ? String.Empty : "modal nofollow";
        }

        static string GetBodyId_(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionDescriptor.ActionName;

            return (controllerName + "_" + actionName).ToLowerInvariant();
        }
    }
}
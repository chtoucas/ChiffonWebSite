namespace Chiffon.Common.Filters
{
    using System;
    using System.Web.Mvc;

    public sealed class HtmlFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["BodyId"] = GetBodyId_(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var identity = filterContext.RequestContext.HttpContext.User.Identity;

            filterContext.Controller.ViewData["RegisterLink"]
                = identity.IsAuthenticated ? String.Empty : "modal nofollow";
        }

        static string GetBodyId_(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;

            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;

            return (controllerName + "_" + actionName).ToLowerInvariant();
        }
    }
}
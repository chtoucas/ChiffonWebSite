namespace Chiffon.Common.Filters
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Infrastructure;

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

            viewData["Language"] = ChiffonEnvironment.Current.Culture.Language;
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
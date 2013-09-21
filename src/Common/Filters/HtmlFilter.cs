namespace Chiffon.Common.Filters
{
    using System;
    using System.Web.Mvc;

    public sealed class HtmlFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;

            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;

            filterContext.Controller.ViewData["ControllerName"] = controllerName;
            filterContext.Controller.ViewData["ActionName"] = actionName;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var identity = filterContext.RequestContext.HttpContext.User.Identity;

            filterContext.Controller.ViewData["ModalRelation"]
                = identity.IsAuthenticated ? String.Empty : "modal:open";
        }
    }
}
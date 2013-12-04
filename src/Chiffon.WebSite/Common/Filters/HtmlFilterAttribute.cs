namespace Chiffon.Common.Filters
{
    using System;
    using System.Web.Mvc;
    using Narvalo;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class HtmlFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            var actionDescriptor = filterContext.ActionDescriptor;

            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;

            filterContext.Controller.ViewData["ControllerName"] = controllerName;
            filterContext.Controller.ViewData["ActionName"] = actionName;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            var identity = filterContext.RequestContext.HttpContext.User.Identity;

            filterContext.Controller.ViewData["ModalRelation"]
                = identity.IsAuthenticated ? String.Empty : "modal:open";
        }
    }
}
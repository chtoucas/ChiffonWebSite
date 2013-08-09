namespace Chiffon.Controllers
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;
    using Chiffon.Common;

    public abstract class BaseController : Controller
    {
        protected IIdentity Identity
        {
            get { return User.Identity; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.Title = "Pour quel motif, Simone ?";
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.ModalClass = Identity.IsAuthenticated ? String.Empty : CssClassName.Modal;
        }
    }
}

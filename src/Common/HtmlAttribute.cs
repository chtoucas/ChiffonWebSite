namespace Chiffon.Common
{
    using System;
    using System.Threading;
    using System.Web.Mvc;
    using Narvalo;

    public sealed class HtmlAttribute : ActionFilterAttribute
    {
        readonly string _bodyId;

        public HtmlAttribute(string bodyId)
        {
            Requires.NotNullOrEmpty(bodyId, "bodyId");

            _bodyId = bodyId;
        }

        public string BodyId { get { return _bodyId; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["BodyId"] = BodyId;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewData = filterContext.Controller.ViewData;
            var identity = filterContext.RequestContext.HttpContext.User.Identity;

            viewData["Language"] = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName; ;
            viewData["RegisterLink"] = identity.IsAuthenticated ? String.Empty : "modal nofollow";
        }
    }
}
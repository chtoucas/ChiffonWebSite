namespace Chiffon.Controllers
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using System.Web.Mvc;
    using Chiffon.Resources;

    public abstract class BaseController : Controller
    {
        protected const string IndexAndFollow = "index, follow";

        const string DefaultMetaRobots_ = "noindex, nofollow";

        protected IIdentity Identity
        {
            get { return User.Identity; }
        }

        // TODO: log !
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.Language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;;
            ViewBag.RegisterLink = Identity.IsAuthenticated ? String.Empty : "modal nofollow";

            // > SEO <

            if (String.IsNullOrEmpty(ViewBag.Title)) {
                ViewBag.Title = "Pour quel motif, Simone ?";
            }
            if (String.IsNullOrEmpty(ViewBag.Description)) {
                ViewBag.Description = SR.MetaDescription;
            }
            if (String.IsNullOrEmpty(ViewBag.Keywords)) {
                ViewBag.Keywords = SR.MetaKeywords;
            }
            if (String.IsNullOrEmpty(ViewBag.Robots)) {
                ViewBag.Robots = DefaultMetaRobots_;
            }
        }
    }
}

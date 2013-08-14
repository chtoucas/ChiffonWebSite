namespace Chiffon.Common.Filters
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Chiffon.Resources;
    using Serilog;

    // TODO: log & check in debug mode !
    // TODO: canonical attribute.
    public sealed class SeoPolicyAttribute : ActionFilterAttribute
    {
        string _robotsDirective = "noindex, nofollow";

        public SeoPolicyAttribute() { }

        public string RobotsDirective { get { return _robotsDirective; } set { _robotsDirective = value; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["MetaRobots"] = RobotsDirective;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            var viewData = filterContext.Controller.ViewData;

            // TODO: ajouter l'en-tête Canonical ?
            // TODO: ajouter dynamiquement la balise meta ?
            __CheckCanonicalLink(filterContext);

            if (String.IsNullOrEmpty(viewBag.MetaDescription)) {
                __Log("No description given, using default.");
                viewData["MetaDescription"] = SR.MetaDescription;
            }
            if (String.IsNullOrEmpty(viewBag.MetaKeywords)) {
                __Log("No keywords given, using default.");
                viewData["MetaKeywords"] = SR.MetaKeywords;
            }
            if (String.IsNullOrEmpty(viewBag.Title)) {
                __Log("No title given, using default.");
                viewData["Title"] = "Pour quel motif, Simone ?";
            }
        }

        [Conditional("DEBUG")]
        void __CheckCanonicalLink(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.IsDebuggingEnabled) {
                if (String.IsNullOrEmpty(filterContext.Controller.ViewBag.CanonicalLink)) {
                    __Log("No canonical link given.");
                }
            }
        }

        [Conditional("DEBUG")]
        void __Log(string message)
        {
            Log.Debug(message);
        }
    }
}
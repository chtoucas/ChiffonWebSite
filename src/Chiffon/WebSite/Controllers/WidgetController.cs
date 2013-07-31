namespace Chiffon.WebSite.Controllers
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Crosscuttings;

    // TODO: Add output cache.
    public class WidgetController : Controller
    {
        readonly ChiffonConfig _config;

        public WidgetController(ChiffonConfig config)
        {
            _config = config;
        }

        [ChildActionOnly]
        public PartialViewResult MemberMenu()
        {
            return PartialView("~/Views/Shared/_MemberMenu.cshtml");
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public PartialViewResult CommonJavaScript()
        {
            return _config.DebugJs
                ? PartialView("~/Views/Widget/Debug/JavaScript.cshtml")
                : PartialView("~/Views/Widget/Release/JavaScript.cshtml");
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public PartialViewResult CommonStylesheet()
        {
            return _config.DebugCss
                ? PartialView("~/Views/Widget/Debug/Stylesheet.cshtml")
                : PartialView("~/Views/Widget/Release/Stylesheet.cshtml");
        }
    }
}

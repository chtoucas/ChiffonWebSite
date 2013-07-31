namespace Chiffon.WebSite.Controllers
{
    using System.Web.Mvc;
    using Chiffon.CrossCuttings;

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
        public PartialViewResult CommonJavaScript()
        {
            return _config.DebugScript
                ? PartialView("~/Views/Widget/Debug/JavaScript.cshtml")
                : PartialView("~/Views/Widget/Release/JavaScript.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult CommonStylesheet()
        {
            return _config.DebugStyle
                ? PartialView("~/Views/Widget/Debug/Stylesheet.cshtml")
                : PartialView("~/Views/Widget/Release/Stylesheet.cshtml");
        }
    }
}

namespace Chiffon.Controllers
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;

    public class WidgetController : Controller
    {
        readonly ChiffonConfig _config;

        public WidgetController(ChiffonConfig config)
        {
            _config = config;
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "bodyId")]
        public virtual PartialViewResult CommonJavaScript(string bodyId)
        {
            return _config.DebugJs
                ? PartialView(ViewName.Widget.CommonJavaScript_Debug, bodyId)
                : PartialView(ViewName.Widget.CommonJavaScript_Release, bodyId);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public virtual PartialViewResult CommonStylesheet()
        {
            return _config.DebugCss
                ? PartialView(ViewName.Widget.CommonStylesheet_Debug)
                : PartialView(ViewName.Widget.CommonStylesheet_Release);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue)]
        public virtual PartialViewResult GoogleAnalytics()
        {
            return PartialView(ViewName.Widget.GoogleAnalytics, _config.GoogleAnalytics);
        }
    }
}

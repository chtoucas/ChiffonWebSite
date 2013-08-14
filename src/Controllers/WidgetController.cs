namespace Chiffon.Controllers
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;

    public partial class WidgetController : Controller
    {
        readonly ChiffonConfig _config;

        public WidgetController(ChiffonConfig config)
        {
            _config = config;
        }

        //[ChildActionOnly]
        //public PartialViewResult MemberMenu()
        //{
        //    return PartialView(ViewName.Widget.MemberMenu);
        //}

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public PartialViewResult CommonJavaScript()
        {
            return _config.DebugJs
                ? PartialView(ViewName.Widget.CommonJavaScript_Debug)
                : PartialView(ViewName.Widget.CommonJavaScript_Release);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public PartialViewResult CommonStylesheet()
        {
            return _config.DebugCss
                ? PartialView(ViewName.Widget.CommonStylesheet_Debug)
                : PartialView(ViewName.Widget.CommonStylesheet_Release);
        }
    }
}

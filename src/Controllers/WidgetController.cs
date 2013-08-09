﻿namespace Chiffon.Controllers
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Crosscuttings;

    public partial class WidgetController : Controller
    {
        readonly ChiffonConfig _config;

        public WidgetController(ChiffonConfig config)
        {
            _config = config;
        }

        [ChildActionOnly]
        public virtual PartialViewResult MemberMenu()
        {
            return PartialView("~/Views/Shared/_MemberMenu.cshtml");
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public virtual PartialViewResult CommonJavaScript()
        {
            return _config.DebugJs
                ? PartialView("~/Views/Widget/Debug/JavaScript.cshtml")
                : PartialView("~/Views/Widget/Release/JavaScript.cshtml");
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public virtual PartialViewResult CommonStylesheet()
        {
            return _config.DebugCss
                ? PartialView("~/Views/Widget/Debug/Stylesheet.cshtml")
                : PartialView("~/Views/Widget/Release/Stylesheet.cshtml");
        }
    }
}

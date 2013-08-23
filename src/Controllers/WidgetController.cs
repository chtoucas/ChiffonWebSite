namespace Chiffon.Controllers
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class WidgetController : Controller
    {
        readonly ChiffonConfig _config;

        public WidgetController(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "bodyId")]
        public PartialViewResult CommonJavaScript(string bodyId)
        {
            return _config.DebugJs
                ? PartialView(ViewName.Widget.CommonJavaScript_Debug, bodyId)
                : PartialView(ViewName.Widget.CommonJavaScript_Release, bodyId);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public PartialViewResult CommonStylesheet()
        {
            return _config.DebugCss
                ? PartialView(ViewName.Widget.CommonStylesheet_Debug)
                : PartialView(ViewName.Widget.CommonStylesheet_Release);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue)]
        public PartialViewResult GoogleAnalytics()
        {
            if (String.IsNullOrEmpty(_config.GoogleAnalytics)) {
                return new EmptyPartialViewResult();
            }

            return PartialView(ViewName.Widget.GoogleAnalytics, _config.GoogleAnalytics);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "language")]
        public PartialViewResult LanguageMenu(ChiffonLanguage language)
        {
            return PartialView(ViewUtility.Localize(ViewName.Widget.LanguageMenu, language));
        }
    }
}

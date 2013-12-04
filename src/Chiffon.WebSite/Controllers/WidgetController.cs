namespace Chiffon.Controllers
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.UI.Assets;

    public class WidgetController : Controller
    {
        readonly ChiffonConfig _config;

        public WidgetController(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        [ChildActionOnly]
        public PartialViewResult CommonJavaScript(string controllerName, string actionName, string languageName)
        {
            var model = new CommonJavaScriptViewModel {
                ActionName = actionName,
                ControllerName = controllerName,
                IsAuthenticated = User.Identity.IsAuthenticated ? "true" : "false",
                LanguageName = languageName,
                ScriptBase = AssetManager.ScriptBase.ToProtocolLessString(),
                Version = _config.JavaScriptVersion,
            };

            return _config.DebugJavaScript
                ? PartialView(ViewName.Widget.CommonJavaScriptDebug, model)
                : PartialView(ViewName.Widget.CommonJavaScriptRelease, model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public PartialViewResult CommonStyleSheet()
        {
            var model = new CommonStyleSheetViewModel {
                Version = _config.CssVersion,
            };

            return _config.DebugStyleSheet
                ? PartialView(ViewName.Widget.CommonStyleSheetDebug)
                : PartialView(ViewName.Widget.CommonStyleSheetRelease, model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue)]
        public PartialViewResult GoogleAnalytics()
        {
            if (String.IsNullOrEmpty(_config.GoogleAnalyticsKey)) {
                return new EmptyPartialViewResult();
            }

            return PartialView(ViewName.Widget.GoogleAnalytics, _config.GoogleAnalyticsKey);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "language")]
        public PartialViewResult LanguageMenu(ChiffonLanguage language)
        {
            return PartialView(ViewUtility.Localize(ViewName.Widget.LanguageMenu, language));
        }
    }
}

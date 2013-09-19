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
                BaseUrl = AssetManager.GetScript(String.Empty),
                ControllerName = controllerName,
                IsAuthenticated = User.Identity.IsAuthenticated ? "true" : "false",
                LanguageName = languageName,
                Version = _config.JsVersion,
            };

            return _config.DebugJs
                ? PartialView(ViewName.Widget.CommonJavaScript_Debug, model)
                : PartialView(ViewName.Widget.CommonJavaScript_Release, model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "none")]
        public PartialViewResult CommonStylesheet()
        {
            var model = new CommonStylesheetViewModel {
                Version = _config.CssVersion,
            };

            return _config.DebugCss
                ? PartialView(ViewName.Widget.CommonStylesheet_Debug)
                : PartialView(ViewName.Widget.CommonStylesheet_Release, model);
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

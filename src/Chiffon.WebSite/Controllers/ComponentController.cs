namespace Chiffon.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.UI.Assets;

    public class ComponentController : Controller
    {
        readonly ChiffonConfig _config;
        readonly ChiffonEnvironment _environment;

        public ComponentController(ChiffonConfig config, ChiffonEnvironment environment)
        {
            Requires.NotNull(config, "config");

            _config = config;
            _environment = environment;
        }

        [ChildActionOnly]
        public PartialViewResult CommonJavaScript(string controllerName, string actionName)
        {
            var model = new CommonJavaScriptViewModel {
                ActionName = actionName,
                ControllerName = controllerName,
                IsAuthenticated = User.Identity.IsAuthenticated ? "true" : "false",
                LanguageName = _environment.UICulture.TwoLetterISOLanguageName,
                ScriptBase = AssetManager.ScriptBase.ToProtocolLessString(),
                Version = _config.JavaScriptVersion,
            };

            return _config.DebugJavaScript
                ? PartialView(Constants.ViewName.Component.CommonJavaScriptDebug, model)
                : PartialView(Constants.ViewName.Component.CommonJavaScriptRelease, model);
        }

        [ChildActionOnly]
        public PartialViewResult LanguageMenu(
            ChiffonLanguage language, 
            IEnumerable<KeyValuePair<ChiffonLanguage, Uri>> alternateUrls)
        {
            return PartialView(ViewUtility.Localize(Constants.ViewName.Component.LanguageMenu, language), alternateUrls);
        }
    }
}

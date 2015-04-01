namespace Chiffon.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.Mvc;

    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web;
    using Narvalo.Web.UI.Assets;

    public sealed class ComponentController : Controller
    {
        private readonly ChiffonConfig _config;

        public ComponentController(ChiffonConfig config)
        {
            Require.NotNull(config, "config");

            _config = config;
        }

        [ChildActionOnly]
        public PartialViewResult CommonJavaScript(string controllerName, string actionName)
        {
            var model = new CommonJavaScriptViewModel {
                ActionName = actionName,
                ControllerName = controllerName,
                IsAuthenticated = User.Identity.IsAuthenticated ? "true" : "false",
                LanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                ScriptBase = AssetManager.ScriptBase.ToProtocolRelativeString(),
                Version = _config.JavaScriptVersion,
            };

            return _config.DebugJavaScript
                ? PartialView(Constants.ViewName.Component.CommonJavaScriptDebug, model)
                : PartialView(Constants.ViewName.Component.CommonJavaScriptRelease, model);
        }

        [ChildActionOnly]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public PartialViewResult LanguageMenu(
            ChiffonLanguage language,
            IEnumerable<KeyValuePair<ChiffonLanguage, Uri>> alternateUrls)
        {
            return PartialView(ViewUtility.Localize(Constants.ViewName.Component.LanguageMenu, language), alternateUrls);
        }
    }
}

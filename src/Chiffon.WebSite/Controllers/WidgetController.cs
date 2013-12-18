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

    public class WidgetController : Controller
    {
        readonly ChiffonConfig _config;
        readonly ChiffonEnvironment _environment;

        public WidgetController(ChiffonConfig config, ChiffonEnvironment environment)
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
                ? PartialView(Constants.ViewName.Widget.CommonJavaScriptDebug, model)
                : PartialView(Constants.ViewName.Widget.CommonJavaScriptRelease, model);
        }

        [ChildActionOnly]
        public PartialViewResult LanguageMenu(
            ChiffonLanguage language, 
            IEnumerable<KeyValuePair<ChiffonLanguage, Uri>> alternateUrls)
        {
            return PartialView(ViewUtility.Localize(Constants.ViewName.Widget.LanguageMenu, language), alternateUrls);
        }

        #region Actions mises en cache.

        // WARNING: Tous les résultats de ces actions sont mis en cache, il est donc important
        // de préciser en paramètre tout ce qui pourrait varier.
        // Toutes les actions suivantes ne peuvent pas accéder à _environment qui dépend de la
        // requête en cours.

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "language")]
        public PartialViewResult AuthorsRights(ChiffonLanguage language)
        {
            return PartialView(ViewUtility.Localize(Constants.ViewName.Widget.AuthorsRights, language));
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue)]
        public PartialViewResult CommonStyleSheet()
        {
            var model = new CommonStyleSheetViewModel {
                Version = _config.CssVersion,
            };

            return _config.DebugStyleSheet
                ? PartialView(Constants.ViewName.Widget.CommonStyleSheetDebug)
                : PartialView(Constants.ViewName.Widget.CommonStyleSheetRelease, model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue)]
        public PartialViewResult Copyright()
        {
            return PartialView(Constants.ViewName.Widget.Copyright);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue)]
        public PartialViewResult GoogleAnalytics()
        {
            if (String.IsNullOrEmpty(_config.GoogleAnalyticsKey)) {
                return new EmptyPartialViewResult();
            }

            return PartialView(Constants.ViewName.Widget.GoogleAnalytics, _config.GoogleAnalyticsKey);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue)]
        public PartialViewResult Html5Shim()
        {
            return PartialView(Constants.ViewName.Widget.Html5Shim);
        }

        [ChildActionOnly]
        [OutputCache(Duration = Int32.MaxValue, VaryByParam = "language")]
        public PartialViewResult Title(ChiffonLanguage language)
        {
            return PartialView(ViewUtility.Localize(Constants.ViewName.Widget.Title, language));
        }

        #endregion
    }
}

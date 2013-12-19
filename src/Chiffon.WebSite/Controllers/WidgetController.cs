namespace Chiffon.Controllers
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web;

    // WARNING: Tous les résultats de ces actions sont mis en cache.
    public class WidgetController : Controller
    {
        readonly ChiffonConfig _config;

        public WidgetController(ChiffonConfig config)
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

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
    }
}

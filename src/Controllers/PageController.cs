namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;

    [HtmlFilter]
    [SeoPolicy]
    public class PageController : Controller
    {
        readonly ChiffonEnvironment _environment;
        readonly ISiteMap _siteMap;

        public PageController(ChiffonEnvironment environment, ISiteMap siteMap)
        {
            Requires.NotNull(environment, "environment");
            Requires.NotNull(siteMap, "siteMap");

            _environment = environment;
            _siteMap = siteMap;
        }

        protected ChiffonCulture Culture { get { return Environment.Culture; } }
        protected ChiffonEnvironment Environment { get { return _environment; } }
        //protected string LanguageName { get { return Environment.Culture.LanguageName; } }
        protected ISiteMap SiteMap { get { return _siteMap; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Language = Environment.Language;
            ViewBag.LanguageName = Culture.LanguageName;
        }
    }
}
namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;

    [HtmlFilter]
    [SeoPolicy]
    public class PageController : Controller
    {
        readonly ChiffonEnvironment _environment;

        public PageController(ChiffonEnvironment environment)
        {
            Requires.NotNull(environment, "environment");

            _environment = environment;
        }

        protected ChiffonEnvironment Environment { get { return _environment; } }
        protected string LanguageName { get { return Environment.Culture.LanguageName; } }
        protected ISiteMap SiteMap { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SiteMap = HttpContext.GetSiteMap();
        }
    }
}
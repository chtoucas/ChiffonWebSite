namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;

    [HtmlFilter]
    [SeoPolicy]
    public class PageController : Controller
    {
        protected string Language { get; private set; }
        protected ISiteMap SiteMap { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Language = ChiffonEnvironment.Current.Culture.Language;
            SiteMap = HttpContext.GetSiteMap();
        }
    }
}
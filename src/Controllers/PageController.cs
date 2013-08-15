namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure.Addressing;

    [HtmlFilter]
    [SeoPolicy]
    public class PageController : Controller
    {
        ISiteMap _siteMap;

        protected ISiteMap SiteMap
        {
            get
            {
                if (_siteMap == null) {
                    _siteMap = HttpContext.GetSiteMap();
                }
                return _siteMap;
            }
        }
    }
}
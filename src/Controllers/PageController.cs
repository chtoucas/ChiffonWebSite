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
                    _siteMap = SiteMapUtility.GetSiteMap(HttpContext);
                }
                return _siteMap;
            }
        }

        //public void SetTitle(string title)
        //{
        //    ViewBag.Title = title;
        //}

        //public void SetMetaDescription(string description)
        //{
        //    ViewBag.MetaDescription = description;
        //}

        //public void SetMetaKeywords(string keywords)
        //{
        //    ViewBag.MetaKeywords = keywords;
        //}
    }
}
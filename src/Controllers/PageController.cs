namespace Chiffon.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;

    [HtmlFilter]
    [SeoPolicy]
    public class PageController : Controller
    {
        readonly ISiteMapFactory _siteMapFactory;
        ISiteMap _siteMap;

        public PageController(ISiteMapFactory siteMapFactory)
        {
            Requires.NotNull(siteMapFactory, "siteMapFactory");

            _siteMapFactory = siteMapFactory;
        }

        protected CultureInfo CurrentCulture { get { return Thread.CurrentThread.CurrentUICulture; } }

        protected ISiteMap SiteMap
        {
            get
            {
                if (_siteMap == null) {
                    _siteMap = _siteMapFactory.CreateMap(CurrentCulture);
                }
                return _siteMap;
            }
        }

        public void SetTitle(string title)
        {
            ViewBag.Title = title;
        }

        public void SetMetaDescription(string description)
        {
            ViewBag.MetaDescription = description;
        }

        public void SetMetaKeywords(string keywords)
        {
            ViewBag.MetaKeywords = keywords;
        }
    }
}
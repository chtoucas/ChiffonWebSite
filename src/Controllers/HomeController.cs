namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Narvalo;

    [SeoPolicy(RobotsDirective = "index, follow")]
    public class HomeController : PageController
    {
        readonly DataContext _dataContext;

        public HomeController(ChiffonEnvironment environment, ISiteMap siteMap, DataContext dataContext)
            : base(environment, siteMap)
        {
            Requires.NotNull(dataContext, "dataContext");

            _dataContext = dataContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = _dataContext.GetHomeView();

            ViewBag.Title = SR.Home_Index_Title;
            ViewBag.MetaDescription = SR.Home_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Home().ToString();

            return View(ViewName.Home.Index, model);
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Title = SR.Home_About_Title;
            ViewBag.MetaDescription = SR.Home_About_Description;
            ViewBag.CanonicalLink = SiteMap.About().ToString();

            return View(ViewName.Home.About);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Title = SR.Home_Contact_Title;
            ViewBag.MetaDescription = SR.Home_Contact_Description;
            ViewBag.CanonicalLink = SiteMap.Contact().ToString();

            return View(ViewName.Home.Contact);
        }

        [HttpGet]
        public ActionResult Newsletter()
        {
            ViewBag.Title = SR.Home_Newsletter_Title;
            ViewBag.MetaDescription = SR.Home_Newsletter_Description;
            ViewBag.CanonicalLink = SiteMap.Newsletter().ToString();

            return View(ViewName.Home.Newsletter);
        }
    }
}

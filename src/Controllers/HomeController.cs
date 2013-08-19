namespace Chiffon.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Chiffon.ViewModels;
    using Narvalo;

    [SeoPolicy(RobotsDirective = "index, follow")]
    public class HomeController : PageController
    {
        readonly IQueries _queries;

        public HomeController(ChiffonEnvironment environment, ISiteMap siteMap, IQueries queries)
            : base(environment, siteMap)
        {
            Requires.NotNull(queries, "queries");

            _queries = queries;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<PatternViewItem> model = _queries.GetHomeViewModel();

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

            // TODO: Généraliser la résolution de vues. Cf. aussi HtmlHelperExtensions
            switch (Environment.Language) {
                case ChiffonLanguage.English:
                    return View(ViewName.Home.AboutEnglish);
                case ChiffonLanguage.Default:
                default:
                    return View(ViewName.Home.About);
            }
        }

        [HttpGet]
        public ActionResult Contact()
        {
            var model = _queries.ListDesigners(LanguageName).OrderBy(_ => _.Lastname);

            ViewBag.Title = SR.Home_Contact_Title;
            ViewBag.MetaDescription = SR.Home_Contact_Description;
            ViewBag.CanonicalLink = SiteMap.Contact().ToString();

            return View(ViewName.Home.Contact, model);
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

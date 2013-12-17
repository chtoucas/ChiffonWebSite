namespace Chiffon.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Narvalo;
    using Narvalo.Web.Semantic;

    //[SeoPolicy(RobotsDirective = "index, follow")]
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
            var designers = _queries.ListDesigners(UICulture);
            var patterns = _queries.ListShowcasedPatterns();
            var model = (from p in patterns
                         join d in designers on p.DesignerKey equals d.Key
                         select ObjectMapper.Map(p, d.DisplayName)).ToList();

            model.Shuffle();

            Ontology.Title = SR.Home_Index_Title;
            Ontology.Description = SR.Home_Index_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Home();

            ViewBag.MainMenuClass = "index";

            return View(ViewName.Home.Index, model);
        }

        [HttpGet]
        public ActionResult About()
        {
            Ontology.Title = SR.Home_About_Title;
            Ontology.Description = SR.Home_About_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.About();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.AboutPage;

            ViewBag.MainMenuClass = "about";

            return View(ViewName.Home.About);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            var model = _queries.ListDesigners(UICulture).OrderBy(_ => _.Nickname.ValueOrElse(_.LastName));

            Ontology.Title = SR.Home_Contact_Title;
            Ontology.Description = SR.Home_Contact_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Contact();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.ContactPage;

            ViewBag.MainMenuClass = "contact";

            return View(ViewName.Home.Contact, model);
        }
    }
}

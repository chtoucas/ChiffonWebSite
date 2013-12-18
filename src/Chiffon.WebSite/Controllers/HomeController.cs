namespace Chiffon.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Chiffon.Resources;
    using Narvalo;
    using Narvalo.Web.Semantic;
    using Addressing = Chiffon.Infrastructure.Addressing;

    [OntologyFilter(RobotsDirective = "index, follow")]
    public class HomeController : ChiffonController
    {
        readonly IQueries _queries;

        public HomeController(ChiffonEnvironment environment, Addressing.ISiteMap siteMap, IQueries queries)
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

            // Ontologie.
            Ontology.Title = SR.Home_Index_Title;
            Ontology.Description = SR.Home_Index_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Home();

            // ViewInfo.
            ViewInfo.AddAlternateUrls(Environment.Language, _ => _.Home());
            ViewInfo.MainMenuClass = "index";

            return View(Constants.ViewName.Home.Index, model);
        }

        [HttpGet]
        public ActionResult About()
        {
            // Ontologie.
            Ontology.Title = SR.Home_About_Title;
            Ontology.Description = SR.Home_About_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.About();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.AboutPage;

            // ViewInfo.
            ViewInfo.AddAlternateUrls(Environment.Language, _ => _.About());
            ViewInfo.MainMenuClass = "about";

            return View(Constants.ViewName.Home.About);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            var model = _queries.ListDesigners(UICulture).OrderBy(_ => _.Nickname.ValueOrElse(_.LastName));

            // Ontologie.
            Ontology.Title = SR.Home_Contact_Title;
            Ontology.Description = SR.Home_Contact_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Contact();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.ContactPage;

            // ViewInfo.
            ViewInfo.AddAlternateUrls(Environment.Language, _ => _.Contact());
            ViewInfo.MainMenuClass = "contact";

            return View(Constants.ViewName.Home.Contact, model);
        }
    }
}

namespace Chiffon.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mail;
    using System.Web;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Data;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Messaging;
    using Chiffon.Resources;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.Semantic;
    using Addressing = Chiffon.Infrastructure.Addressing;

    [OntologyFilter(RobotsDirective = "index, follow")]
    public class HomeController : ChiffonController
    {
        readonly IMessenger _messenger;
        readonly IQueries _queries;

        public HomeController(ChiffonEnvironment environment, Addressing.ISiteMap siteMap, IMessenger messenger, IQueries queries)
            : base(environment, siteMap)
        {
            Requires.NotNull(messenger, "messenger");
            Requires.NotNull(queries, "queries");

            _messenger = messenger;
            _queries = queries;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Modèle.
            var designers = _queries.ListDesigners(CultureInfo.CurrentUICulture);
            var patterns = _queries.ListShowcasedPatterns();
            var model = (from p in patterns
                         join d in designers on p.DesignerKey equals d.Key
                         select ObjectMapper.Map(p, d.DisplayName)).ToList();

            model.Shuffle();

            // Ontologie.
            Ontology.Title = SR.Home_Index_Title;
            Ontology.Description = SR.Home_Index_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Home();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Home());
            if (User.Identity.IsAuthenticated) {
                LayoutViewModel.MainHeading = SR.Home_Index_MainHeading;
            }
            LayoutViewModel.MainNavCssClass = "index";

            return View(Constants.ViewName.Home.Index, model);
        }

        [HttpGet]
        public ActionResult About()
        {
            // Modèle.
            var model = _queries.ListDesigners(CultureInfo.CurrentUICulture).OrderBy(_ => _.Nickname.ValueOrElse(_.LastName));

            // Ontologie.
            Ontology.Title = SR.Home_About_Title;
            Ontology.Description = SR.Home_About_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.About();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.AboutPage;

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.About());
            LayoutViewModel.MainHeading = SR.Home_About_MainHeading;
            LayoutViewModel.MainNavCssClass = "about";

            return View(Constants.ViewName.Home.About, model);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            // Modèle.
            var model = new ContactViewModel();

            if (User.Identity.IsAuthenticated) {
                model.Name = User.Identity.Name;
                model.Email = MemberSession.Value.Email;
            }

            // Ontologie.
            Ontology.Title = SR.Home_Contact_Title;
            Ontology.Description = SR.Home_Contact_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Contact();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.ContactPage;

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Contact());
            LayoutViewModel.MainHeading = SR.Home_Contact_MainHeading;
            LayoutViewModel.MainNavCssClass = "contact";

            return View(Constants.ViewName.Home.Contact, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel model)
        {
            Requires.NotNull(model, "model");

            // Ontologie.
            Ontology.Title = SR.Home_Contact_Title;
            Ontology.Description = SR.Home_Contact_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Contact();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.ContactPage;

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Contact());
            LayoutViewModel.MainHeading = SR.Home_Contact_MainHeading;
            LayoutViewModel.MainNavCssClass = "contact";

            if (!ModelState.IsValid) {
                return View(Constants.ViewName.Home.Contact, model);
            }

            _messenger.Publish(new NewContactMessage {
                ContactAddress = new MailAddress(model.Email, model.Name),
                Content = model.Message,
            });

            return RedirectToRoute(Constants.RouteName.Home.ContactSuccess);
        }

        [HttpGet]
        [OntologyFilter(RobotsDirective = "noindex, nofollow")]
        public ActionResult ContactSuccess()
        {
            // Ontologie.
            Ontology.Relationships.CanonicalUrl = SiteMap.ContactSuccess();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Contact());
            LayoutViewModel.MainHeading = SR.Home_ContactSuccess_MainHeading;
            LayoutViewModel.MainNavCssClass = "contact";

            return View(Constants.ViewName.Home.ContactSuccess);
        }
    }
}

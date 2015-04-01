namespace Chiffon.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mail;
    using System.Web;
    using System.Web.Mvc;

    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Persistence;
    using Chiffon.Properties;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.Semantic;

    public sealed class HomeController : ChiffonController
    {
        private readonly IMessenger _messenger;
        private readonly IDbQueries _queries;

        public HomeController(ChiffonEnvironment environment, ISiteMap siteMap, IMessenger messenger, IDbQueries queries)
            : base(environment, siteMap)
        {
            Require.NotNull(messenger, "messenger");
            Require.NotNull(queries, "queries");

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
            Ontology.Title = Strings.Home_Index_Title;
            Ontology.Description = Strings.Home_Index_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Home();

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Home());
            if (User.Identity.IsAuthenticated)
            {
                LayoutViewModel.MainHeading = Strings.Home_Index_MainHeading;
            }

            LayoutViewModel.MainMenuCssClass = "index";

            return View(Constants.ViewName.Home.Index, model);
        }

        [HttpGet]
        public ActionResult About()
        {
            // Modèle.
            var model = _queries.ListDesigners(CultureInfo.CurrentUICulture).OrderBy(_ => _.Nickname.ValueOrElse(_.LastName));

            // Ontologie.
            Ontology.Title = Strings.Home_About_Title;
            Ontology.Description = Strings.Home_About_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.About();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.AboutPage;

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.About());
            LayoutViewModel.MainHeading = Strings.Home_About_MainHeading;
            LayoutViewModel.MainMenuCssClass = "about";

            return View(Constants.ViewName.Home.About, model);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            // Modèle.
            var model = new ContactViewModel();

            if (User.Identity.IsAuthenticated)
            {
                model.Name = User.Identity.Name;

                // IMPORTANT: On doit vérifier si la session n'a pas disparue.
                // Cf. la remarque en début de la classe MemberSession.
                var session = MemberSession.Value;
                if (session != null)
                {
                    model.Email = session.Email;
                }
            }

            // Ontologie.
            Ontology.Title = Strings.Home_Contact_Title;
            Ontology.Description = Strings.Home_Contact_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Contact();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.ContactPage;

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Contact());
            LayoutViewModel.MainHeading = Strings.Home_Contact_MainHeading;
            LayoutViewModel.MainMenuCssClass = "contact";

            return View(Constants.ViewName.Home.Contact, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel model)
        {
            Require.NotNull(model, "model");

            // Ontologie.
            Ontology.Title = Strings.Home_Contact_Title;
            Ontology.Description = Strings.Home_Contact_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Contact();
            Ontology.SchemaOrg.ItemType = SchemaOrgType.ContactPage;

            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Contact());
            LayoutViewModel.MainHeading = Strings.Home_Contact_MainHeading;
            LayoutViewModel.MainMenuCssClass = "contact";

            if (!ModelState.IsValid)
            {
                return View(Constants.ViewName.Home.Contact, model);
            }

            _messenger.Publish(new NewContactMessage {
                EmailAddress = new MailAddress(model.Email, model.Name),
                MessageContent = model.Message,
            });

            return RedirectToRoute(Constants.RouteName.Home.ContactSuccess);
        }

        [HttpGet]
        [OntologyFilter(Disabled = true)]
        public ActionResult ContactSuccess()
        {
            // LayoutViewModel.
            LayoutViewModel.AddAlternateUrls(Environment.Language, _ => _.Contact());
            LayoutViewModel.MainHeading = Strings.Home_ContactSuccess_MainHeading;
            LayoutViewModel.MainMenuCssClass = "contact";

            return View(Constants.ViewName.Home.ContactSuccess);
        }
    }
}

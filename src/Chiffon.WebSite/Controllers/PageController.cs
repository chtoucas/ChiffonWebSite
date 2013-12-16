namespace Chiffon.Controllers
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Narvalo;
    using Narvalo.Web.Semantic;
    using Narvalo.Web.UI.Assets;

    [HtmlFilter]
    [OntologyFilter]
    [SeoPolicy]
    public class PageController : Controller
    {
        readonly ChiffonEnvironment _environment;
        readonly Ontology _ontology;
        readonly ISiteMap _siteMap;

        protected PageController(ChiffonEnvironment environment, ISiteMap siteMap)
        {
            Requires.NotNull(siteMap, "siteMap");

            _environment = environment;
            _siteMap = siteMap;

            _ontology = new Ontology(Culture.UICulture);
        }

        protected ChiffonCulture Culture { get { return Environment.Culture; } }
        protected ChiffonEnvironment Environment { get { return _environment; } }
        protected Ontology Ontology { get { return _ontology; } }
        protected ISiteMap SiteMap { get { return _siteMap; } }

        protected ActionResult LocalizedView(string viewName)
        {
            return View(ViewUtility.Localize(viewName, Environment.Language));
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            InitializeOntology_();

            ViewBag.Language = Environment.Language;
            ViewBag.Ontology = Ontology;
        }

        void InitializeOntology_()
        {
            // Il semble que "Keywords" est ignoré par Google, il n'est donc 
            // pas nécessaire de travailler cet aspect là.
            _ontology.Keywords = SR.DefaultKeywords;

            _ontology.OpenGraph.SiteName = SR.DefaultTitle;

            // Par défaut, on utilise le logo comme image.
            _ontology.OpenGraph.Image = new OpenGraphImage {
                Height = 144,
                MimeType = OpenGraphImage.PngMimeType,
                Url = AssetManager.GetImage("logo.png"),
                Width = 144,
            };

            // Autres langues dans lesquelles la page est disponible.
            foreach (var environment in ChiffonEnvironment.Environments) {
                if (environment.Language == Environment.Language) {
                    continue;
                }
                _ontology.OpenGraph.AddAlternativeLocale(new OpenGraphLocale(environment.Culture.UICulture));
            }
        }
    }
}
namespace Chiffon.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
        static IEnumerable<OpenGraphLocale> OpenGraphLocales_;

        readonly ChiffonEnvironment _environment;
        readonly Ontology _ontology;
        readonly ISiteMap _siteMap;

        protected PageController(ChiffonEnvironment environment, ISiteMap siteMap)
        {
            Requires.NotNull(siteMap, "siteMap");

            _environment = environment;
            _siteMap = siteMap;

            _ontology = new Ontology(UICulture);
        }

        protected static IEnumerable<OpenGraphLocale> OpenGraphLocales
        {
            get
            {
                if (OpenGraphLocales_ == null) {
                    OpenGraphLocales_ = from env in ChiffonEnvironmentResolver.Environments
                                        select new OpenGraphLocale(env.UICulture);
                }
                return OpenGraphLocales_;
            }
        }

        protected CultureInfo UICulture { get { return Environment.UICulture; } }
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
            _ontology.OpenGraph.Image = new OpenGraphImage(AssetManager.GetImage("logo.png")) {
                Height = 144,
                MimeType = OpenGraphImage.PngMimeType,
                Width = 144,
            };

            // Autres langues dans lesquelles la page est disponible.
            var alternativeLocales = from _ in OpenGraphLocales
                                     where _ != _ontology.OpenGraph.Locale
                                     select _;
            _ontology.OpenGraph.AddAlternativeLocales(alternativeLocales);
        }
    }
}
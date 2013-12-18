namespace Chiffon.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;
    using Narvalo.Web.Semantic;

    [OntologyFilter]
    public abstract class ChiffonController : Controller
    {
        static readonly Lazy<IEnumerable<ISiteMap>> SiteMaps_
            = new Lazy<IEnumerable<ISiteMap>>(() =>
            {
                return from env in ChiffonEnvironmentResolver.Environments
                       select new DefaultSiteMap(env);
            });

        readonly ChiffonControllerContext _chiffonControllerContext;
        readonly ChiffonEnvironment _environment;
        readonly Ontology _ontology;
        readonly ISiteMap _siteMap;

        protected ChiffonController(ChiffonEnvironment environment, ISiteMap siteMap)
        {
            Requires.NotNull(siteMap, "siteMap");

            _environment = environment;
            _siteMap = siteMap;

            _ontology = new Ontology(UICulture);

            _chiffonControllerContext = new ChiffonControllerContext() {
                Environment = _environment,
                Ontology = _ontology,
            };
        }

        protected static IEnumerable<ISiteMap> SiteMaps { get { return SiteMaps_.Value; } }

        public ChiffonControllerContext ChiffonControllerContext { get { return _chiffonControllerContext; } }

        protected ChiffonEnvironment Environment { get { return _environment; } }
        protected Ontology Ontology { get { return _ontology; } }
        protected CultureInfo UICulture { get { return Environment.UICulture; } }
        protected ISiteMap SiteMap { get { return _siteMap; } }

        protected void AddAlternateUrlsToViewBag(Func<ISiteMap, Uri> fun)
        {
            ViewBag.AlternateUrls = GetAlternateUrls(fun);
        }

        protected void AddMainMenuClassToViewBag(string className)
        {
            ViewBag.MainMenuClass = className;
        }

        protected void AddReturnUrlToViewBag(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
        }

        protected IEnumerable<KeyValuePair<ChiffonLanguage, Uri>> GetAlternateUrls(Func<ISiteMap, Uri> fun)
        {
            return from s in SiteMaps 
                   where s.Language != Environment.Language
                   select new KeyValuePair<ChiffonLanguage, Uri>(s.Language, fun(s));
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            var actionDescriptor = filterContext.ActionDescriptor;

            ViewBag.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            ViewBag.ActionName = actionDescriptor.ActionName;
            ViewBag.MainMenuClass = String.Empty;
        }
    }
}
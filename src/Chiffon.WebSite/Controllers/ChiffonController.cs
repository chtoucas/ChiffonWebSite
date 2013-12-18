namespace Chiffon.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;
    using Narvalo.Web.Semantic;

    [OntologyFilter]
    public class ChiffonController : Controller
    {
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

        public ChiffonControllerContext ChiffonControllerContext { get { return _chiffonControllerContext; } }

        protected ChiffonEnvironment Environment { get { return _environment; } }
        protected Ontology Ontology { get { return _ontology; } }
        protected CultureInfo UICulture { get { return Environment.UICulture; } }
        protected ISiteMap SiteMap { get { return _siteMap; } }

        //protected ActionResult LocalizedView(string viewName)
        //{
        //    return View(ViewUtility.Localize(viewName, Environment.Language));
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            var actionDescriptor = filterContext.ActionDescriptor;

            ChiffonControllerContext.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            ChiffonControllerContext.ActionName = actionDescriptor.ActionName;
        }
    }
}
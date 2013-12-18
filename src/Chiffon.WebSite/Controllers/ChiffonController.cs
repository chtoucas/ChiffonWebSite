namespace Chiffon.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;
    using Narvalo.Web.Semantic;

    [OntologyFilter]
    public abstract class ChiffonController : Controller
    {
        readonly ChiffonControllerContext _chiffonControllerContext;
        readonly ChiffonEnvironment _environment;
        readonly Ontology _ontology;
        readonly ISiteMap _siteMap;
        readonly ViewInfo _viewInfo;

        protected ChiffonController(ChiffonEnvironment environment, ISiteMap siteMap)
        {
            Requires.NotNull(siteMap, "siteMap");

            _environment = environment;
            _siteMap = siteMap;

            _ontology = new Ontology(UICulture);
            _viewInfo = new ViewInfo(ViewData);

            _chiffonControllerContext = new ChiffonControllerContext() {
                Environment = _environment,
                Ontology = _ontology,
            };
        }

        public ChiffonControllerContext ChiffonControllerContext { get { return _chiffonControllerContext; } }

        protected ChiffonEnvironment Environment { get { return _environment; } }
        protected Ontology Ontology { get { return _ontology; } }
        protected ISiteMap SiteMap { get { return _siteMap; } }
        protected CultureInfo UICulture { get { return Environment.UICulture; } }
        protected ViewInfo ViewInfo { get { return _viewInfo; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            var actionDescriptor = filterContext.ActionDescriptor;

            ViewInfo.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            ViewInfo.ActionName = actionDescriptor.ActionName;
        }
    }
}
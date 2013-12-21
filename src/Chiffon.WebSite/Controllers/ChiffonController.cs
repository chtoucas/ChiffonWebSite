﻿namespace Chiffon.Controllers
{
    using System.Globalization;
    using System.Web.Mvc;
    using Chiffon.Common.Filters;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.Semantic;

    [OntologyFilter]
    public abstract class ChiffonController : Controller
    {
        readonly LayoutViewModel _layoutViewModel = new LayoutViewModel();

        readonly ChiffonControllerContext _chiffonControllerContext;
        readonly ChiffonEnvironment _environment;
        readonly Ontology _ontology;
        readonly ISiteMap _siteMap;

        protected ChiffonController(ChiffonEnvironment environment, ISiteMap siteMap)
        {
            Requires.NotNull(siteMap, "siteMap");

            _environment = environment;
            _siteMap = siteMap;

            _ontology = new Ontology(CultureInfo.CurrentUICulture);

            _chiffonControllerContext = new ChiffonControllerContext() {
                Language = _environment.Language,
                LayoutViewModel = _layoutViewModel,
                Ontology = _ontology,
            };
        }

        public ChiffonControllerContext ChiffonControllerContext { get { return _chiffonControllerContext; } }

        protected ChiffonEnvironment Environment { get { return _environment; } }
        protected LayoutViewModel LayoutViewModel { get { return _layoutViewModel; } }
        protected Ontology Ontology { get { return _ontology; } }
        protected ISiteMap SiteMap { get { return _siteMap; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            var actionDescriptor = filterContext.ActionDescriptor;

            LayoutViewModel.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            LayoutViewModel.ActionName = actionDescriptor.ActionName;
        }

        protected ActionResult RedirectToHome()
        {
            return RedirectToRoute(Common.Constants.RouteName.Home.Index);
        }
    }
}
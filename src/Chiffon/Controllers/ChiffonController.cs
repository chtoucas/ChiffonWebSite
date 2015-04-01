namespace Chiffon.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.Semantic;

    [OntologyFilter]
    public abstract class ChiffonController : Controller
    {
        private readonly LayoutViewModel _layoutViewModel = new LayoutViewModel();

        private readonly ChiffonControllerContext _chiffonControllerContext;
        private readonly ChiffonEnvironment _environment;
        private readonly Ontology _ontology;
        private readonly ISiteMap _siteMap;

        private Lazy<MemberSession> _memberSesssion;

        protected ChiffonController(ChiffonEnvironment environment, ISiteMap siteMap)
        {
            Require.NotNull(siteMap, "siteMap");

            _environment = environment;
            _siteMap = siteMap;

            _ontology = new Ontology(CultureInfo.CurrentUICulture);

            _chiffonControllerContext = new ChiffonControllerContext() {
                Language = _environment.Language,
                LayoutViewModel = _layoutViewModel,
                Ontology = _ontology,
            };

            _memberSesssion = new Lazy<MemberSession>(GetMemberSessionThunk_(this));
        }

        public ChiffonControllerContext ChiffonControllerContext { get { return _chiffonControllerContext; } }

        protected ChiffonEnvironment Environment { get { return _environment; } }

        protected LayoutViewModel LayoutViewModel { get { return _layoutViewModel; } }

        protected MemberSession MemberSession { get { return _memberSesssion.Value; } }

        protected Ontology Ontology { get { return _ontology; } }

        protected ISiteMap SiteMap { get { return _siteMap; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Require.NotNull(filterContext, "filterContext");

            var actionDescriptor = filterContext.ActionDescriptor;

            LayoutViewModel.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            LayoutViewModel.ActionName = actionDescriptor.ActionName;
        }

        protected ActionResult RedirectToHome()
        {
            return RedirectToRoute(Constants.RouteName.Home.Index);
        }

        private static Func<MemberSession> GetMemberSessionThunk_(Controller @this)
        {
            return () =>
            {
                return new MemberSession(@this.HttpContext.Session);
            };
        }
    }
}
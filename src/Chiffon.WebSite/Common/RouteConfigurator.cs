namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Chiffon.Handlers;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;
    using Narvalo.Web;

    // TODO: I18N.
    public class RouteConfigurator
    {
        readonly RouteCollection _routes;

        public RouteConfigurator(RouteCollection routes)
        {
            Require.NotNull(routes, "routes");

            _routes = routes;
        }

        public void Configure()
        {
            _routes.IgnoreRoute("devil/elmah");
            _routes.IgnoreRoute("devil/glimpse");
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // HomeController.
            _routes.MapRoute(Constants.RouteName.Home.Index, String.Empty,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Index });
            _routes.MapRoute(Constants.RouteName.Home.About, DefaultSiteMap.Constants.About,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.About });
            _routes.MapRoute(Constants.RouteName.Home.Contact, DefaultSiteMap.Constants.Contact,
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.Contact });
            _routes.MapRoute(Constants.RouteName.Home.ContactSuccess, "contact-confirmation",
                new { controller = Constants.ControllerName.Home, action = Constants.ActionName.Home.ContactSuccess });

            // AccountController.
            _routes.MapRoute(Constants.RouteName.Account.Register, DefaultSiteMap.Constants.Register,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Register });
            _routes.MapRoute(Constants.RouteName.Account.RegisterSuccess, "inscription-confirmation",
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.RegisterSuccess });
            _routes.MapRoute(Constants.RouteName.Account.Login, DefaultSiteMap.Constants.Login,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Login });
            _routes.MapRoute(Constants.RouteName.Account.Newsletter, DefaultSiteMap.Constants.Newsletter,
                new { controller = Constants.ControllerName.Account, action = Constants.ActionName.Contact.Newsletter });

            // DesignerController.
            _routes.MapRoute(Constants.RouteName.Designer.Index, DefaultSiteMap.Constants.Designer,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Index },
                new { designerKey = new DesignerKeyConstraint() });
            _routes.MapRoute(Constants.RouteName.Designer.Category, DefaultSiteMap.Constants.DesignerCategory,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Category },
                new { designerKey = new DesignerKeyConstraint() });
            _routes.MapRoute(Constants.RouteName.Designer.Pattern, DefaultSiteMap.Constants.DesignerPattern,
                new { controller = Constants.ControllerName.Designer, action = Constants.ActionName.Designer.Pattern },
                new { designerKey = new DesignerKeyConstraint() });

            // MailController.
            _routes.MapRoute(Constants.RouteName.MailMerge.Welcome, "mail/bienvenue",
                new { controller = Constants.ControllerName.MailMerge, action = Constants.ActionName.MailMerge.Welcome });
            _routes.MapRoute(Constants.RouteName.MailMerge.ForgottenPassword, "mail/mot-de-passe-oublié",
               new { controller = Constants.ControllerName.MailMerge, action = Constants.ActionName.MailMerge.ForgottenPassword });

            _routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            RegisterHandlerRoutes_();
        }

        void RegisterHandlerRoutes_()
        {
            _routes.Add(new Route(DefaultSiteMap.Constants.Pattern, new AutofacRouteHandler<PatternImageHandler>()));
            _routes.Add(new Route(DefaultSiteMap.Constants.LogOn, new AutofacRouteHandler<LogOnHandler>()));
            _routes.Add(new Route(DefaultSiteMap.Constants.LogOff, new AutofacRouteHandler<LogOffHandler>()));
            //_routes.Add(new Route("go", new AutofacRouteHandler<GoHandler>()));
        }
    }
}
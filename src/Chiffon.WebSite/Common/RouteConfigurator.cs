namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Chiffon.Handlers;
    using Narvalo;
    using Narvalo.Web;

    // TODO: I18N.
    public class RouteConfigurator
    {
        readonly RouteCollection _routes;

        public RouteConfigurator(RouteCollection routes)
        {
            Requires.NotNull(routes, "routes");

            _routes = routes;
        }

        public void Configure()
        {
            _routes.IgnoreRoute("admin/elmah");
            _routes.IgnoreRoute("admin/glimpse");
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // HomeController.
            _routes.MapRoute(RouteName.Home.Index, String.Empty,
                new { controller = ControllerName.Home, action = ActionName.Home.Index });
            _routes.MapRoute(RouteName.Home.About, "informations",
                new { controller = ControllerName.Home, action = ActionName.Home.About });
            _routes.MapRoute(RouteName.Home.Contact, "contact",
                new { controller = ControllerName.Home, action = ActionName.Home.Contact });

            // AccountController.
            _routes.MapRoute(RouteName.Account.Register, "inscription",
                new { controller = ControllerName.Account, action = ActionName.Contact.Register });
            _routes.MapRoute(RouteName.Account.Login, "connexion",
                new { controller = ControllerName.Account, action = ActionName.Contact.Login });
            _routes.MapRoute(RouteName.Account.Newsletter, "newsletter",
                new { controller = ControllerName.Account, action = ActionName.Contact.Newsletter });

            // DesignerController.
            _routes.MapRoute(RouteName.Designer.Index, "{designerKey}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Index },
                new { designerKey = new DesignerKeyConstraint()});
            _routes.MapRoute(RouteName.Designer.Category, "{designerKey}/{categoryKey}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Category },
                new { designerKey = new DesignerKeyConstraint()});
            _routes.MapRoute(RouteName.Designer.Pattern, "{designerKey}/{categoryKey}/{reference}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Pattern },
                new { designerKey = new DesignerKeyConstraint()});

            _routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            RegisterHandlerRoutes_();
        }

        void RegisterHandlerRoutes_()
        {
            _routes.Add(new Route("motif", new AutofacRouteHandler<PatternImageHandler>()));
            _routes.Add(new Route("connecte", new AutofacRouteHandler<LogOnHandler>()));
            _routes.Add(new Route("disconnecte", new AutofacRouteHandler<LogOffHandler>()));
            _routes.Add(new Route("go", new AutofacRouteHandler<GoHandler>()));
        }
    }
}
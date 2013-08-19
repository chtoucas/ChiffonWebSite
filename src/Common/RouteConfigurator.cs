namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Chiffon.Entities;
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
            _routes.MapRoute(RouteName.Home.Newsletter, "newsletter",
                new { controller = ControllerName.Home, action = ActionName.Home.Newsletter });

            // AccountController.
            _routes.MapRoute(RouteName.Account.Register, "inscription",
                new { controller = ControllerName.Account, action = ActionName.Account.Register });
            _routes.MapRoute(RouteName.Account.Login, "connexion",
                new { controller = ControllerName.Account, action = ActionName.Account.Login });

            // DesignerController.
            _routes.MapRoute(RouteName.EstherMarthi.Index, "chicamancha/",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Index, designerKey = DesignerKey.EstherMarthi });
            _routes.MapRoute(RouteName.VivianeDevaux.Index, "viviane-devaux/",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Index, designerKey = DesignerKey.VivianeDevaux });
            _routes.MapRoute(RouteName.ChristineLégeret.Index, "petroleum-blue/",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Index, designerKey = DesignerKey.ChristineLégeret });
            _routes.MapRoute(RouteName.LaureRoussel.Index, "laure-roussel/",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Index, designerKey = DesignerKey.LaureRoussel });

            _routes.MapRoute(RouteName.EstherMarthi.Category, "chicamancha/{categoryKey}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Category, designerKey = DesignerKey.EstherMarthi });
            _routes.MapRoute(RouteName.VivianeDevaux.Category, "viviane-devaux/{categoryKey}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Category, designerKey = DesignerKey.VivianeDevaux });
            _routes.MapRoute(RouteName.ChristineLégeret.Category, "petroleum-blue/{categoryKey}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Category, designerKey = DesignerKey.ChristineLégeret });
            _routes.MapRoute(RouteName.LaureRoussel.Category, "laure-roussel/{categoryKey}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Category, designerKey = DesignerKey.LaureRoussel });

            _routes.MapRoute(RouteName.EstherMarthi.Pattern, "chicamancha/{categoryKey}/{reference}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Pattern, designerKey = DesignerKey.EstherMarthi });
            _routes.MapRoute(RouteName.VivianeDevaux.Pattern, "viviane-devaux/{categoryKey}/{reference}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Pattern, designerKey = DesignerKey.VivianeDevaux });
            _routes.MapRoute(RouteName.ChristineLégeret.Pattern, "petroleum-blue/{categoryKey}/{reference}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Pattern, designerKey = DesignerKey.ChristineLégeret });
            _routes.MapRoute(RouteName.LaureRoussel.Pattern, "laure-roussel/{categoryKey}/{reference}",
                new { controller = ControllerName.Designer, action = ActionName.Designer.Pattern, designerKey = DesignerKey.LaureRoussel });

            _routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            RegisterRouteHandlers_();
        }

        void RegisterRouteHandlers_()
        {
            _routes.Add(new Route("motif", new AutofacRouteHandler<PatternImageHandler>()));
            _routes.Add(new Route("connecte", new AutofacRouteHandler<LogOnHandler>()));
            _routes.Add(new Route("disconnecte", new AutofacRouteHandler<LogOffHandler>()));
        }
    }
}
namespace Chiffon.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
                new { controller = "Home", action = "Index" });
            _routes.MapRoute(RouteName.Home.About, "informations",
                new { controller = "Home", action = "About" });
            _routes.MapRoute(RouteName.Home.Contact, "contact",
                new { controller = "Home", action = "Contact" });
            _routes.MapRoute(RouteName.Home.Newsletter, "newsletter",
                new { controller = "Home", action = "Newsletter" });

            // AccountController.
            _routes.MapRoute(RouteName.Account.Register, "inscription",
                new { controller = "Account", action = "Register" });
            _routes.MapRoute(RouteName.Account.Login, "connexion",
                new { controller = "Account", action = "Login" });

            // DesignerController.
            _routes.MapRoute(RouteName.Chicamancha.Index, "chicamancha/",
                new { controller = "Designer", action = "Index", designer = DesignerKey.Chicamancha });
            _routes.MapRoute(RouteName.VivianeDevaux.Index, "viviane-devaux/",
                new { controller = "Designer", action = "Index", designer = DesignerKey.VivianeDevaux });
            _routes.MapRoute(RouteName.ChristineLégeret.Index, "christine-legeret/",
                new { controller = "Designer", action = "Index", designer = DesignerKey.ChristineLégeret });
            _routes.MapRoute(RouteName.LaureRoussel.Index, "laure-roussel/",
                new { controller = "Designer", action = "Index", designer = DesignerKey.LaureRoussel });

            _routes.MapRoute(RouteName.Chicamancha.Pattern, "chicamancha/{reference}",
                new { controller = "Designer", action = "Pattern", designer = DesignerKey.Chicamancha });
            _routes.MapRoute(RouteName.VivianeDevaux.Pattern, "viviane-devaux/{reference}",
                new { controller = "Designer", action = "Pattern", designer = DesignerKey.VivianeDevaux });
            _routes.MapRoute(RouteName.ChristineLégeret.Pattern, "christine-legeret/{reference}",
                new { controller = "Designer", action = "Pattern", designer = DesignerKey.ChristineLégeret });
            _routes.MapRoute(RouteName.LaureRoussel.Pattern, "laure-roussel/{reference}",
                new { controller = "Designer", action = "Pattern", designer = DesignerKey.LaureRoussel });

            _routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            Handlers_();
        }

        void Handlers_()
        {
            _routes.Add(new Route("PatternImage.ashx", new AutofacRouteHandler<PatternImageHandler>()));
            _routes.Add(new Route("connecte", new AutofacRouteHandler<LogOnHandler>()));
            _routes.Add(new Route("deconnecte", new AutofacRouteHandler<LogOffHandler>()));
        }
    }
}
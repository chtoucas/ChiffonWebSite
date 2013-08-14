namespace Chiffon.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
            _routes.IgnoreRoute("elmah.axd");
            _routes.IgnoreRoute("glimpse.axd");
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

            // DesignerController.
            _routes.MapRoute(RouteName.Chicamancha.Index, "chicamancha/",
                new { controller = "Designer", action = "Index", designer = "chicamancha" });
            _routes.MapRoute(RouteName.VivianeDevaux.Index, "viviane-devaux/",
                new { controller = "Designer", action = "Index", designer = "viviane-devaux" });
            _routes.MapRoute(RouteName.ChristineLégeret.Index, "christine-legeret/",
                new { controller = "Designer", action = "Index", designer = "christine-legeret" });
            _routes.MapRoute(RouteName.LaureRoussel.Index, "laure-roussel/",
                new { controller = "Designer", action = "Index", designer = "laure-roussel" });

            _routes.MapRoute(RouteName.Chicamancha.Pattern, "chicamancha/{reference}",
                new { controller = "Designer", action = "Pattern", designer = "chicamancha" });
            _routes.MapRoute(RouteName.VivianeDevaux.Pattern, "viviane-devaux/{reference}",
                new { controller = "Designer", action = "Pattern", designer = "viviane-devaux" });
            _routes.MapRoute(RouteName.ChristineLégeret.Pattern, "christine-legeret/{reference}",
                new { controller = "Designer", action = "Pattern", designer = "christine-legeret" });
            _routes.MapRoute(RouteName.LaureRoussel.Pattern, "laure-roussel/{reference}",
                new { controller = "Designer", action = "Pattern", designer = "laure-roussel" });

            _routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            Handlers_();
        }

        void Handlers_()
        {
            _routes.Add(new Route("PatternImage.ashx", new AutofacRouteHandler<PatternImageHandler>()));
        }
    }
}
﻿namespace Chiffon.Common
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
            _routes.IgnoreRoute("elmah.axd");
            _routes.IgnoreRoute("glimpse.axd");
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // HomeController.
            _routes.MapRoute(RouteName.Home, String.Empty, new { controller = MVC.Home.Name, action = "Index" });
            _routes.MapRoute(RouteName.About, "informations", new { controller = MVC.Home.Name, action = "About" });
            _routes.MapRoute(RouteName.Contact, "contact", new { controller = MVC.Home.Name, action = "Contact" });
            _routes.MapRoute(RouteName.Newsletter, "newsletter", new { controller = MVC.Home.Name, action = "Newsletter" });

            // AccountController.
            _routes.MapRoute(RouteName.Register, "inscription", new { controller = MVC.Account.Name, action = "Register" });

            // MemberController.
            _routes.MapRoute(RouteName.Chicamancha, "chicamancha/{action}", new { controller = MVC.Member.Name, action = "Index", designer = "chicamancha" });
            _routes.MapRoute(RouteName.VivianeDevaux, "viviane-devaux/{action}", new { controller = MVC.Member.Name, action = "Index", designer = "viviane-devaux" });
            _routes.MapRoute(RouteName.ChristineLégeret, "christine-legeret/{action}", new { controller = MVC.Member.Name, action = "Index", designer = "christine-legeret" });
            _routes.MapRoute(RouteName.LaureRoussel, "laure-roussel/{action}", new { controller = MVC.Member.Name, action = "Index", designer = "laure-roussel" });

            _routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            Handlers_();
        }

        void Handlers_()
        {
            _routes.Add(new Route("PatternImage.ashx", new AutofacRouteHandler<PatternImageHandler>()));
        }
    }
}
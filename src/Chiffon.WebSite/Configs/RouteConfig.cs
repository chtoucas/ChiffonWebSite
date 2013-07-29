namespace Chiffon.WebSite.Configs
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Narvalo;

    // TODO: I18N.
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Requires.NotNull(routes, "routes");

            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("glimpse.axd");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // HomeController.
            routes.MapRoute("Home", String.Empty, new { controller = "Home", action = "Index" });
            routes.MapRoute("About", "informations", new { controller = "Home", action = "About" });
            routes.MapRoute("Contact", "contact", new { controller = "Home", action = "Contact" });

            // MemberController.
            routes.MapRoute("Chicamancha", "chicamancha/{action}", new { controller = "Member", action = "Index", key = "cm" });
            routes.MapRoute("VivianeDevaux", "viviane-devaux/{action}", new { controller = "Member", action = "Index", key = "vd" });
            routes.MapRoute("ChristineLégeret", "christine-légeret/{action}", new { controller = "Member", action = "Index", key = "cm" });
            routes.MapRoute("LaureRoussel", "laure-roussel/{action}", new { controller = "Member", action = "Index", key = "lr" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
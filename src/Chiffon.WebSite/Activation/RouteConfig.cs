namespace Chiffon.WebSite.Activation
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Narvalo;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Requires.NotNull(routes, "routes");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "About",
                url: "informations",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "Home", action = "Contact" }
            );

            routes.MapRoute(
                name: "Chicamancha",
                url: "chicamancha",
                defaults: new { controller = "Member", action = "Index" }
            );

            routes.MapRoute(
                name: "VivianeDevaux",
                url: "viviane-devaux",
                defaults: new { controller = "Member", action = "Index" }
            );

            routes.MapRoute(
                name: " ChristineLégeret",
                url: "christine-légeret",
                defaults: new { controller = "Member", action = "Index" }
            );

            routes.MapRoute(
                name: "LaureRoussel",
                url: "laure-roussel",
                defaults: new { controller = "Member", action = "Index" }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
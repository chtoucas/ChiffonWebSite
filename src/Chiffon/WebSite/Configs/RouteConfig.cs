namespace Chiffon.WebSite.Configs
{
    using System;
    using System.Linq;
    using System.Reflection;
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

            //routes.MapRoute("CommonJavaScript", String.Empty, new { controller = "Asset", action = "CommonJavaScript" });
            //routes.MapRoute("CommonStylesheet", String.Empty, new { controller = "Asset", action = "CommonStylesheet" });

            routes.MapChildOnlyActionRoutesFrom(typeof(Global).Assembly);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }

        // http://www.make-awesome.com/2012/07/perfectionist-routing-in-asp-net-mvc/
        public static void MapChildOnlyActionRoutesFrom(this RouteCollection routes, Assembly assembly)
        {
            foreach (Type t in assembly.GetTypes().Where(t => !t.IsAbstract && typeof(IController).IsAssignableFrom(t))) {
                bool allChildOnly = t.GetCustomAttributes(typeof(ChildActionOnlyAttribute), true).Any();

                foreach (MethodInfo m in t.GetMethods()) {
                    if (m.IsPublic && typeof(ActionResult).IsAssignableFrom(m.ReturnType)) {
                        if (allChildOnly || m.GetCustomAttributes(typeof(ChildActionOnlyAttribute), true).Any()) {
                            string controller = t.Name;
                            string action = m.Name;

                            if (controller.EndsWith("Controller"))
                                controller = controller.Substring(0, controller.Length - 10);

                            string name = String.Format("ChildAction/{0}/{1}", controller, action);
                            var constraints = new
                            {
                                controller = controller,
                                action = action
                            };

                            routes.MapRoute(name, String.Empty, null, constraints);
                        }
                    }
                }
            }
        }

        //public static void MapCatchAllErrorThrowingDefaultRoute(this RouteCollection routes)
        //{
        //    routes.Add("Default", new RestrictiveCatchAllDefaultRoute());
        //}

        //public class RestrictiveCatchAllDefaultRoute : Route
        //{
        //    public RestrictiveCatchAllDefaultRoute()
        //        : base("*", new MvcRouteHandler())
        //    {
        //        DataTokens = new RouteValueDictionary(new { warning = "routes.MapCatchAllErrorThrowingDefaultRoute() must be the very last mapped route because it will catch everything and throw exceptions!" });
        //    }

        //    public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        //    {
        //        string valueDebug = String.Join(String.Empty, values.Select(p => String.Format("\r\n{0} = {1}", p.Key, p.Value)).ToArray());
        //        throw new InvalidOperationException("Unable to find a valid route for the following route values:" + valueDebug);
        //    }
        //}
    }
}
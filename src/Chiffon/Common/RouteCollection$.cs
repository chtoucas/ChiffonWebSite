namespace Chiffon.Common
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    // http://www.make-awesome.com/2012/07/perfectionist-routing-in-asp-net-mvc/
    public static class RouteCollectionExtensions
    {
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
                            var constraints = new {
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

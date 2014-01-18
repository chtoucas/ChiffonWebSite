namespace Chiffon
{
    using System.Web.Routing;
    using Chiffon.Handlers;
    using Narvalo;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Require.NotNull(routes, "routes");

            routes.Add(new Route("motif", new AutofacRouteHandler<PatternImageHandler>()));
        }
    }
}
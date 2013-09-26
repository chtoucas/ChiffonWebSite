namespace Chiffon
{
    using System.Web.Routing;
    using Chiffon.Common;
    using Narvalo;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Requires.NotNull(routes, "routes");

            new RouteConfigurator(routes).Configure();
        }
    }
}
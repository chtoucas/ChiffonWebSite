namespace Chiffon
{
    using System.Web.Routing;
    using Chiffon.Common;
    using Narvalo;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Require.NotNull(routes, "routes");

            new RouteConfigurator(routes).Configure();
        }
    }
}
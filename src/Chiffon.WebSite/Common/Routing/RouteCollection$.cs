namespace Chiffon.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteCollectionExtensions
    {
        public static LocalizedRoute MapLocalizedRoute(this RouteCollection routes,
            string name, string url, object defaults, 
            IDictionary<CultureInfo, String> localizedUrls)
        {
            var route = new LocalizedRoute(
                url,
                new RouteValueDictionary(defaults),
                new MvcRouteHandler(),
                localizedUrls);
            routes.Add(name, route);
            return route;
        }

        public static LocalizedRoute MapLocalizedRoute(this RouteCollection routes,
            string name, string url, object defaults, object constraints,
            IDictionary<CultureInfo, String> localizedUrls)
        {
            var route = new LocalizedRoute(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints),
                new MvcRouteHandler(),
                localizedUrls);
            routes.Add(name, route);
            return route;
        }
    }
}

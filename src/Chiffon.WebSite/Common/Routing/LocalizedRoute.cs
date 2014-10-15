namespace Chiffon.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Routing;

    public class LocalizedRoute : Route
    {
        public LocalizedRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler,
            IDictionary<CultureInfo, String> localizedUrls)
            : base(url, defaults, routeHandler)
        {
            LocalizedUrls = localizedUrls;
        }

        public LocalizedRoute(string url, RouteValueDictionary defaults,
            RouteValueDictionary constraints, IRouteHandler routeHandler,
            IDictionary<CultureInfo, String> localizedUrls)
            : base(url, defaults, constraints, routeHandler)
        {
            LocalizedUrls = localizedUrls;
        }

        public IDictionary<CultureInfo, String> LocalizedUrls { get; private set; }

        private CultureInfo CurrentCulture { get { return Thread.CurrentThread.CurrentUICulture; } }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData routeData = base.GetRouteData(httpContext);
            if (routeData == null) { return null; }

            //var translator = LocalizedUrls.First(_ => _.Key == CurrentCulture);

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            RouteValueDictionary translatedValues = values;

            return base.GetVirtualPath(requestContext, translatedValues);
        }
    }
}
namespace Narvalo.Common
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Routing;
    using Narvalo.Collections;

    public class LocalizedRoute : Route
    {
        public LocalizedRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler,
            IDictionary<CultureInfo, RouteValueTranslator> translators)
            : base(url, defaults, routeHandler)
        {
            Translators = translators;
        }

        public LocalizedRoute(string url, RouteValueDictionary defaults,
            RouteValueDictionary constraints, IRouteHandler routeHandler,
            IDictionary<CultureInfo, RouteValueTranslator> translators)
            : base(url, defaults, constraints, routeHandler)
        {
            Translators = translators;
        }

        public IDictionary<CultureInfo, RouteValueTranslator> Translators { get; private set; }
        private CultureInfo CurrentCulture { get { return Thread.CurrentThread.CurrentUICulture; } }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData routeData = base.GetRouteData(httpContext);
            if (routeData == null) { return null; }

            var translator = Translators.First(_ => _.Key == CurrentCulture);
            var value = translator.Value.Translations;

            // Translate route values
            foreach (var translationProvider in Translators) {
                //if (routeData.Values.ContainsKey(pair.Key)) {
                //    RouteValueTranslation translation = translationProvider.TranslateToRouteValue(
                //        routeData.Values[pair.Key].ToString(),
                //        cultureInfo);

                //    routeData.Values[pair.Key] = translation.RouteValue;
                //}
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext,
            RouteValueDictionary values)
        {
            RouteValueDictionary translatedValues = values;

            // Translate route values
            foreach (var translationProvider in this.Translators) {
                //if (translationProvider != null
                //    && translatedValues.ContainsKey(pair.Key)) {
                //    RouteValueTranslation translation =
                //        translationProvider.TranslateToTranslatedValue(
                //            translatedValues[pair.Key].ToString(), cultureInfo);

                //    translatedValues[pair.Key] = translation.TranslatedValue;
                //}
            }

            return base.GetVirtualPath(requestContext, translatedValues);
        }
    }
}
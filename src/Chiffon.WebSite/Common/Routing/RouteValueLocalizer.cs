namespace Narvalo.Common
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class RouteValueLocalizer
    {
        public RouteValueLocalizer(IList<LocalizedRouteValue> translations)
        {
            LocalizedValues = translations;
        }

        public IList<LocalizedRouteValue> LocalizedValues { get; private set; }

        public LocalizedRouteValue TranslateToRouteValue(string translatedValue, CultureInfo cultureInfo)
        {
            LocalizedRouteValue translation = null;

            // Find translation in specified CultureInfo
            translation = LocalizedValues.Where(
                t => t.LocalizedValue == translatedValue
                    && (t.Culture.ToString() == cultureInfo.ToString()
                        || t.Culture.ToString().Substring(0, 2) == cultureInfo.ToString().Substring(0, 2)))
                .OrderByDescending(t => t.Culture)
                .FirstOrDefault();
            if (translation != null) {
                return translation;
            }

            // Find translation without taking account on CultureInfo
            translation = LocalizedValues.Where(t => t.LocalizedValue == translatedValue).FirstOrDefault();
            if (translation != null) {
                return translation;
            }

            // Return the current values
            return new LocalizedRouteValue {
                Culture = cultureInfo,
                RouteValue = translatedValue,
                LocalizedValue = translatedValue
            };
        }

        public LocalizedRouteValue TranslateToTranslatedValue(string routeValue, CultureInfo cultureInfo)
        {
            LocalizedRouteValue translation = null;

            // Find translation in specified CultureInfo
            translation = LocalizedValues.Where(
                t => t.RouteValue == routeValue
                    && (t.Culture.ToString() == cultureInfo.ToString()
                        || t.Culture.ToString().Substring(0, 2) == cultureInfo.ToString().Substring(0, 2)))
                .OrderByDescending(t => t.Culture)
                .FirstOrDefault();

            if (translation != null) {
                return translation;
            }

            // Find translation without taking account on CultureInfo
            translation = LocalizedValues.Where(t => t.RouteValue == routeValue).FirstOrDefault();
            if (translation != null) {
                return translation;
            }

            // Return the current values
            return new LocalizedRouteValue {
                Culture = cultureInfo,
                RouteValue = routeValue,
                LocalizedValue = routeValue
            };
        }
    }
}

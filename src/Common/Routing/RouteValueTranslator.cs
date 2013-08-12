namespace Narvalo.Common
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class RouteValueTranslator
    {
        public RouteValueTranslator(IList<LocalizedRouteValue> translations)
        {
            Translations = translations;
        }

        public IList<LocalizedRouteValue> Translations { get; private set; }

        public LocalizedRouteValue TranslateToRouteValue(string translatedValue, CultureInfo cultureInfo)
        {
            LocalizedRouteValue translation = null;

            // Find translation in specified CultureInfo
            translation = Translations.Where(
                t => t.TranslatedValue == translatedValue
                    && (t.CultureInfo.ToString() == cultureInfo.ToString()
                        || t.CultureInfo.ToString().Substring(0, 2) == cultureInfo.ToString().Substring(0, 2)))
                .OrderByDescending(t => t.CultureInfo)
                .FirstOrDefault();
            if (translation != null) {
                return translation;
            }

            // Find translation without taking account on CultureInfo
            translation = Translations.Where(t => t.TranslatedValue == translatedValue).FirstOrDefault();
            if (translation != null) {
                return translation;
            }

            // Return the current values
            return new LocalizedRouteValue {
                CultureInfo = cultureInfo,
                RouteValue = translatedValue,
                TranslatedValue = translatedValue
            };
        }

        public LocalizedRouteValue TranslateToTranslatedValue(string routeValue, CultureInfo cultureInfo)
        {
            LocalizedRouteValue translation = null;

            // Find translation in specified CultureInfo
            translation = Translations.Where(
                t => t.RouteValue == routeValue
                    && (t.CultureInfo.ToString() == cultureInfo.ToString()
                        || t.CultureInfo.ToString().Substring(0, 2) == cultureInfo.ToString().Substring(0, 2)))
                .OrderByDescending(t => t.CultureInfo)
                .FirstOrDefault();

            if (translation != null) {
                return translation;
            }

            // Find translation without taking account on CultureInfo
            translation = Translations.Where(t => t.RouteValue == routeValue).FirstOrDefault();
            if (translation != null) {
                return translation;
            }

            // Return the current values
            return new LocalizedRouteValue {
                CultureInfo = cultureInfo,
                RouteValue = routeValue,
                TranslatedValue = routeValue
            };
        }
    }
}

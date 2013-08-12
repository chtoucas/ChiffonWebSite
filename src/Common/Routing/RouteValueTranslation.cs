namespace Narvalo.Common
{
    using System.Globalization;

    public class LocalizedRouteValue
    {
        public LocalizedRouteValue() { }

        public LocalizedRouteValue(CultureInfo cultureInfo, string routeValue, string translatedValue)
        {
            CultureInfo = cultureInfo;
            RouteValue = routeValue;
            TranslatedValue = translatedValue;
        }

        public CultureInfo CultureInfo { get; set; }

        public string RouteValue { get; set; }

        public string TranslatedValue { get; set; }
    }
}
namespace Narvalo.Common
{
    using System.Globalization;

    public class LocalizedRouteValue
    {
        public LocalizedRouteValue() { }

        public LocalizedRouteValue(CultureInfo culture, string routeValue, string localizedValue)
        {
            Culture = culture;
            RouteValue = routeValue;
            LocalizedValue = localizedValue;
        }

        public CultureInfo Culture { get; set; }

        public string RouteValue { get; set; }

        public string LocalizedValue { get; set; }
    }
}
namespace Narvalo.Common
{
    using System.Globalization;

    public interface IRouteValueTranslator
    {
        LocalizedRouteValue TranslateToRouteValue(string translatedValue, CultureInfo cultureInfo);
        LocalizedRouteValue TranslateToTranslatedValue(string routeValue, CultureInfo cultureInfo);
    }
}

namespace Narvalo.Web.Semantic
{
    using System.Globalization;
    using Narvalo;

    public struct OpenGraphLocale
    {
        CultureInfo _culture;

        public OpenGraphLocale(CultureInfo culture)
        {
            Requires.NotNull(culture, "culture");

            _culture = culture;
        }

        public override string ToString()
        {
            return _culture.ToString().Replace('-', '_');
        }
    }
}

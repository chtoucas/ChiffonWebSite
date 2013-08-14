namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using System.Globalization;

    public class DefaultSiteMapFactory : ISiteMapFactory
    {
        // TODO: Utiliser un siteMap anglais.
        public ISiteMap CreateMap(CultureInfo culture)
        {
            Uri baseUri;

            if (culture.TwoLetterISOLanguageName == "en") {
                baseUri = new Uri("http://en.pourquelmotifsimone.com");
            }
            else {
                baseUri = new Uri("http://pourquelmotifsimone.com");
            }

            return new DefaultSiteMap(baseUri);
        }
    }
}

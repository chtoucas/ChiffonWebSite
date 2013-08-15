namespace Chiffon.Infrastructure.Addressing
{
    using System.Globalization;

    public class DefaultSiteMapFactory : ISiteMapFactory
    {
        //public ISiteMap CreateMap()
        //{
        //    return CreateMap(Thread.CurrentThread.CurrentUICulture);
        //}

        public ISiteMap CreateMap(CultureInfo culture)
        {
            switch (culture.TwoLetterISOLanguageName) {
                case "en":
                    return new EnglishSiteMap();
                default:
                    return new DefaultSiteMap();
            }
        }
    }
}

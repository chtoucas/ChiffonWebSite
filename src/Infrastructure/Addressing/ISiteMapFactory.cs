namespace Chiffon.Infrastructure.Addressing
{
    using System.Globalization;

    public interface ISiteMapFactory
    {
        ISiteMap CreateMap(CultureInfo culture);
    }
}

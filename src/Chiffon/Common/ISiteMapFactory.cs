namespace Chiffon.Common
{
    using Chiffon.Infrastructure;

    public interface ISiteMapFactory
    {
        ISiteMap CreateMap(ChiffonEnvironment environment);
    }
}

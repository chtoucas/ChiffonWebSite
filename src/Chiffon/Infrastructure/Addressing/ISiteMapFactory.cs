namespace Chiffon.Infrastructure.Addressing
{
    public interface ISiteMapFactory
    {
        ISiteMap CreateMap(ChiffonEnvironment environment);
    }
}

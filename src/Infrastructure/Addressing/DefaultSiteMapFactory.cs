namespace Chiffon.Infrastructure.Addressing
{
    public class DefaultSiteMapFactory : ISiteMapFactory
    {
        public ISiteMap CreateMap(ChiffonEnvironment environment)
        {
            return new DefaultSiteMap(environment.BaseUri);
        }
    }
}

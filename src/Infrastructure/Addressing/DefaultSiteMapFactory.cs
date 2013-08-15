namespace Chiffon.Infrastructure.Addressing
{
    public class DefaultSiteMapFactory : ISiteMapFactory
    {
        public ISiteMap CreateMap(ChiffonEnvironmentBase environment)
        {
            return new DefaultSiteMap(environment.BaseUri);
        }
    }
}

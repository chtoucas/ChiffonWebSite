namespace Chiffon.Infrastructure.Addressing
{
    using Narvalo;

    public class DefaultSiteMapFactory : ISiteMapFactory
    {
        public ISiteMap CreateMap(ChiffonEnvironment environment)
        {
            Requires.NotNull(environment, "environment");

            return new DefaultSiteMap(environment.BaseUri);
        }
    }
}

namespace Chiffon.Infrastructure.Addressing
{
    public sealed class DefaultSiteMapFactory : ISiteMapFactory
    {
        public ISiteMap CreateMap(ChiffonEnvironment environment)
        {
            switch (environment.Hosting) {
                case ChiffonHosting.SingleDomain:
                    return new SingleDomainSiteMap(environment);
                case ChiffonHosting.OneDomainPerLanguage:
                default:
                    return new OneDomainPerLanguageSiteMap(environment);
            }
        }
    }
}

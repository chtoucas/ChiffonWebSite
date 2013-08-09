namespace Chiffon.Infrastructure.Addressing
{
    using System;

    public class SiteMapBuilder
    {
        static readonly SiteMapBuilder _Instance = new SiteMapBuilder();
        Func<ISiteMapFactory> _factoryThunk = () => null;

        public SiteMapBuilder()
        {
        }

        public static SiteMapBuilder Current
        {
            get
            {
                return _Instance;
            }
        }

        public ISiteMapFactory GetSiteMapFactory()
        {
            ISiteMapFactory factory = _factoryThunk();
            return factory;
        }

        public void SetSiteMapFactory(ISiteMapFactory factory)
        {
            _factoryThunk = () => factory;
        }
    }
}

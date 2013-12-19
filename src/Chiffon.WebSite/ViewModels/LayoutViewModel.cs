namespace Chiffon.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;

    [Serializable]
    public class LayoutViewModel
    {
        static readonly Lazy<IEnumerable<ISiteMap>> SiteMaps_
            = new Lazy<IEnumerable<ISiteMap>>(() =>
            {
                return from env in ChiffonEnvironmentResolver.Environments
                       select new DefaultSiteMap(env);
            });

        string _mainNavCssClass = String.Empty;

        public LayoutViewModel() { }

        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public IEnumerable<KeyValuePair<ChiffonLanguage, Uri>> AlternateUrls { get; set; }

        public string MainNavCssClass { get { return _mainNavCssClass; } set { _mainNavCssClass = value; } }
        public string SecondaryNavCssClass { get; set; }

        protected static IEnumerable<ISiteMap> SiteMaps { get { return SiteMaps_.Value; } }

        public void AddAlternateUrls(ChiffonLanguage language, Func<ISiteMap, Uri> fun)
        {
            AlternateUrls = CreateAlternateUrls_(language, fun);
        }

        static IEnumerable<KeyValuePair<ChiffonLanguage, Uri>>
            CreateAlternateUrls_(ChiffonLanguage language, Func<ISiteMap, Uri> fun)
        {
            return from s in SiteMaps
                   where s.Language != language
                   select new KeyValuePair<ChiffonLanguage, Uri>(s.Language, fun(s));
        }
    }
}
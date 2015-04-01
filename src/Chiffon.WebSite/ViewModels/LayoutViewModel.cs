namespace Chiffon.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;

    public sealed class LayoutViewModel
    {
        // FIXME: Vraiment pas efficace et incorrect en plus (on ne sait pas réellement quel est 
        // l'ISiteMap en cours d'utilisation.
        private static readonly Lazy<IEnumerable<ISiteMap>> SiteMaps_
             = new Lazy<IEnumerable<ISiteMap>>(() =>
             {
                 return from env in ChiffonEnvironmentResolver.Environments
                        select new SingleDomainSiteMap(env);
             });

        private bool _disableOntology = false;
        private string _mainHeading = String.Empty;
        private string _mainMenuCssClass = String.Empty;

        public LayoutViewModel() { }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public bool DisableOntology { get { return _disableOntology; } set { _disableOntology = value; } }

        public string MainHeading { get { return _mainHeading; } set { _mainHeading = value; } }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<KeyValuePair<ChiffonLanguage, Uri>> AlternateUrls { get; set; }

        public string MainMenuCssClass { get { return _mainMenuCssClass; } set { _mainMenuCssClass = value; } }

        public string DesignerMenuCssClass { get; set; }

        public void AddAlternateUrls(ChiffonLanguage language, Func<ISiteMap, Uri> fun)
        {
            AlternateUrls = CreateAlternateUrls_(language, fun);
        }

        private static IEnumerable<KeyValuePair<ChiffonLanguage, Uri>> CreateAlternateUrls_(
            ChiffonLanguage language,
            Func<ISiteMap, Uri> fun)
        {
            return from s in SiteMaps_.Value
                   where s.Language != language
                   select new KeyValuePair<ChiffonLanguage, Uri>(s.Language, fun(s));
        }
    }
}
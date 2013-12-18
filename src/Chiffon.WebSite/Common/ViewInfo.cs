namespace Chiffon.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;

    public class ViewInfo
    {
        static readonly Lazy<IEnumerable<ISiteMap>> SiteMaps_
        = new Lazy<IEnumerable<ISiteMap>>(() =>
        {
            return from env in ChiffonEnvironmentResolver.Environments
                   select new DefaultSiteMap(env);
        });

        readonly ViewDataDictionary _viewData;

        public ViewInfo(ViewDataDictionary viewData)
        {
            _viewData = viewData;

            MainMenuClass = String.Empty;
        }

        #region Shared > _Layout.cshtml

        public string ActionName
        {
            get { return _viewData["ActionName"] as String; }
            set { _viewData["ActionName"] = value; }
        }

        public IEnumerable<KeyValuePair<ChiffonLanguage, Uri>> AlternateUrls
        {
            get { return _viewData["AlternateUrls"] as IEnumerable<KeyValuePair<ChiffonLanguage, Uri>>; }
            set { _viewData["AlternateUrls"] = value; }
        }

        public string ControllerName
        {
            get { return _viewData["ControllerName"] as String; }
            set { _viewData["ControllerName"] = value; }
        }

        public string MainMenuClass
        {
            get { return _viewData["MainMenuClass"] as String; }
            set { _viewData["MainMenuClass"] = value; }
        }

        #endregion

        #region Shared > _DesignerInfo.cshtml

        public string CurrentCategoryKey
        {
            get { return _viewData["CurrentCategoryKey"] as String; }
            set { _viewData["CurrentCategoryKey"] = value; }
        }

        public string DesignerClass
        {
            get { return _viewData["DesignerClass"] as String; }
            set { _viewData["DesignerClass"] = value; }
        }

        #endregion

        #region Account > Login.cshtml

        public string ReturnUrl
        {
            get { return _viewData["ReturnUrl"] as String; }
            set { _viewData["ReturnUrl"] = value; }
        }

        #endregion

        protected static IEnumerable<ISiteMap> SiteMaps { get { return SiteMaps_.Value; } }

        public void AddAlternateUrls(ChiffonLanguage language, Func<ISiteMap, Uri> fun)
        {
            AlternateUrls = GetAlternateUrls(language, fun);
        }

        protected static IEnumerable<KeyValuePair<ChiffonLanguage, Uri>>
            GetAlternateUrls(ChiffonLanguage language, Func<ISiteMap, Uri> fun)
        {
            return from s in SiteMaps
                   where s.Language != language
                   select new KeyValuePair<ChiffonLanguage, Uri>(s.Language, fun(s));
        }
    }
}
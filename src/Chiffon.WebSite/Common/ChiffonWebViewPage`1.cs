namespace Chiffon.Common
{
    using System.Web.Mvc;
    using Chiffon.Infrastructure;

    // TODO:
    // - activer l'injection de dépendance.
    // Cf. https://code.google.com/p/autofac/issues/detail?id=349
    // Cf. http://stackoverflow.com/questions/14933450/property-injection-into-custom-webviewpage-using-autofac 
    // - ajouter AssetTag
    public abstract class ChiffonWebViewPage<TModel> : WebViewPage<TModel>
    {
        protected ChiffonEnvironment Environment
        {
            get { return ChiffonContext.Current.Environment; }
        }

        protected string CurrentActionName
        {
            get
            {
                return ViewContext.RouteData.Values["action"].ToString();
            }
        }

        protected string CurrentControllerName
        {
            get
            {
                return ViewContext.RouteData.Values["controller"].ToString();
            }
        }

        //public override void InitHelpers()
        //{
        //    base.InitHelpers();
        //}
    }
}

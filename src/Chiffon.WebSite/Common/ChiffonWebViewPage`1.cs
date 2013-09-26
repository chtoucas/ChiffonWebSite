namespace Chiffon.Common
{
    using System.Web.Mvc;

    // Cf. http://haacked.com/archive/2011/02/21/changing-base-type-of-a-razor-view.aspx
    public abstract class ChiffonWebViewPage<TModel> : WebViewPage<TModel>
    {
        public override void InitHelpers()
        {
            base.InitHelpers();
        }
    }
}

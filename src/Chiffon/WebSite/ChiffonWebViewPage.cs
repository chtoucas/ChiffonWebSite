namespace Chiffon.WebSite
{
    using System.Web.Mvc;

    // Cf. http://haacked.com/archive/2011/02/21/changing-base-type-of-a-razor-view.aspx
    public abstract class ChiffonWebViewPage : WebViewPage
    {
        public override void InitHelpers()
        {
            base.InitHelpers();
        }
    }
}

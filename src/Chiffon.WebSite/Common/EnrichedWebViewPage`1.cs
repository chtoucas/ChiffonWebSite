namespace Chiffon.Common
{
    using System.Web.Mvc;
    using Narvalo.Web.Html;

    public abstract class EnrichedWebViewPage<TModel> : WebViewPage<TModel>
    {
        protected AssetHelper Asset { get; private set; }

        public override void InitHelpers()
        {
            base.InitHelpers();

            Asset = new AssetHelper(Html);
        }
    }
}

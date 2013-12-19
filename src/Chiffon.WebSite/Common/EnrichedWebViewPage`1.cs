namespace Chiffon.Common
{
    using System.Web.Mvc;
    using Narvalo.Web.Html;

    public abstract class EnrichedWebViewPage<TModel> : WebViewPage<TModel>
    {
        protected Tag Tag { get; private set; }

        public override void InitHelpers()
        {
            base.InitHelpers();

            Tag = new Tag();
        }
    }
}

namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Chiffon.Controllers;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo.Web.Html;
    using Narvalo.Web.Semantic;

    // TODO: Utiliser l'injection de dépendance ?
    // Cf. https://code.google.com/p/autofac/issues/detail?id=349
    // Cf. http://stackoverflow.com/questions/14933450/property-injection-into-custom-webviewpage-using-autofac 
    public abstract class ChiffonWebViewPage<TModel> : WebViewPage<TModel>
    {
        Lazy<ChiffonControllerContext> _chiffonControllerContext;

        protected ChiffonWebViewPage()
        {
            _chiffonControllerContext = new Lazy<ChiffonControllerContext>(GetChiffonControlerContextThunk_(this));
        }

        protected ChiffonControllerContext ChiffonControlerContext
        {
            get { return _chiffonControllerContext.Value; }
        }

        protected ChiffonLanguage Language { get { return ChiffonControlerContext.Language; } }
        protected LayoutViewModel LayoutViewModel { get { return ChiffonControlerContext.LayoutViewModel; } }
        protected Ontology Ontology { get { return ChiffonControlerContext.Ontology; } }
        protected AssetHelper Asset { get; private set; }

        public void RenderActionResource(string resourceName)
        {
            Html.RenderAction(ViewUtility.Localize(resourceName, Language), LayoutViewModel.ControllerName);
        }

        public void RenderComponent(string componentName, object routeValues)
        {
            Html.RenderAction(componentName, Constants.ControllerName.Component, routeValues);
        }

        public void RenderPartialResource(string resourceName)
        {
            Html.RenderPartial(ViewUtility.Localize(resourceName, Language));
        }

        public void RenderWidget(string widgetName)
        {
            RenderWidget(widgetName, false /* localized */);
        }

        public void RenderWidget(string widgetName, bool localized)
        {
            if (localized) {
                Html.RenderAction(widgetName, Constants.ControllerName.Widget, new { language = Language });
            }
            else {
                Html.RenderAction(widgetName, Constants.ControllerName.Widget);
            }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            Asset = new AssetHelper(Html);
        }

        internal static Func<ChiffonControllerContext> GetChiffonControlerContextThunk_(WebViewPage @this)
        {
            return () =>
            {
                var controller = @this.ViewContext.Controller as ChiffonController;
                if (controller == null) {
                    throw new InvalidOperationException(
                        "ChiffonWebViewPage should only be used on views for actions on ChiffonController.");
                }
                return controller.ChiffonControllerContext;
            };
        }
    }
}

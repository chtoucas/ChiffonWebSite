namespace Chiffon.Views
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using Chiffon.Common;
    using Chiffon.Controllers;
    using Narvalo.Web.Semantic;

    // TODO: Utiliser l'injection de dépendance ?
    // Cf. https://code.google.com/p/autofac/issues/detail?id=349
    // Cf. http://stackoverflow.com/questions/14933450/property-injection-into-custom-webviewpage-using-autofac 
    public abstract class ChiffonWebViewPage<TModel> : WebViewPage<TModel>
    {
        private Lazy<ChiffonControllerContext> _chiffonControllerContext;
        private Lazy<String> _followRelation;

        protected ChiffonWebViewPage()
        {
            _chiffonControllerContext = new Lazy<ChiffonControllerContext>(GetChiffonControllerContextThunk_(this));
            _followRelation = new Lazy<String>(GetFollowRelationThunk_(this));
        }

        protected ChiffonControllerContext ChiffonControllerContext
        {
            get { return _chiffonControllerContext.Value; }
        }

        protected string FollowRelation
        {
            get { return _followRelation.Value; }
        }

        protected ChiffonLanguage Language { get { return ChiffonControllerContext.Language; } }

        protected LayoutViewModel LayoutViewModel { get { return ChiffonControllerContext.LayoutViewModel; } }

        protected Ontology Ontology { get { return ChiffonControllerContext.Ontology; } }

        public void RenderActionResource(string resourceName)
        {
            Contract.Requires(resourceName != null);

            Html.RenderAction(ViewUtility.Localize(resourceName, Language), LayoutViewModel.ControllerName);
        }

        public void RenderComponent(string componentName, object routeValues)
        {
            Html.RenderAction(componentName, Constants.ControllerName.Component, routeValues);
        }

        public void RenderPartialResource(string resourceName)
        {
            Contract.Requires(resourceName != null);

            Html.RenderPartial(ViewUtility.Localize(resourceName, Language));
        }

        public void RenderWidget(string widgetName)
        {
            RenderWidget(widgetName, false /* localized */);
        }

        public void RenderWidget(string widgetName, bool localized)
        {
            if (localized)
            {
                Html.RenderAction(widgetName, Constants.ControllerName.Widget, new { language = Language });
            }
            else
            {
                Html.RenderAction(widgetName, Constants.ControllerName.Widget);
            }
        }

        private static Func<ChiffonControllerContext> GetChiffonControllerContextThunk_(WebViewPage @this)
        {
            return () =>
            {
                var controller = @this.ViewContext.Controller as ChiffonController;
                if (controller == null)
                {
                    throw new InvalidOperationException(
                        "ChiffonWebViewPage should only be used on views for actions on ChiffonController.");
                }

                return controller.ChiffonControllerContext;
            };
        }

        private static Func<String> GetFollowRelationThunk_(WebViewPage @this)
        {
            return () =>
            {
                return @this.User.Identity.IsAuthenticated ? "follow" : "nofollow";
            };
        }
    }
}

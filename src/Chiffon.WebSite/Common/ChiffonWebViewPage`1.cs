namespace Chiffon.Common
{
    using System;
    using System.Web.Mvc;
    using Chiffon.Controllers;
    using Chiffon.Infrastructure;
    using Narvalo.Web.Semantic;

    // TODO:
    // - activer l'injection de dépendance ?
    // Cf. https://code.google.com/p/autofac/issues/detail?id=349
    // Cf. http://stackoverflow.com/questions/14933450/property-injection-into-custom-webviewpage-using-autofac 
    // - ajouter AssetTag
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

        protected ChiffonEnvironment Environment { get { return ChiffonControlerContext.Environment; } }
        protected Ontology Ontology { get { return ChiffonControlerContext.Ontology; } }

        //public override void InitHelpers()
        //{
        //    base.InitHelpers();
        //}

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

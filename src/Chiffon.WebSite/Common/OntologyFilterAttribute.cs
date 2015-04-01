namespace Chiffon.Common
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;

    using Chiffon.Controllers;
    using Chiffon.Infrastructure;
    using Chiffon.Resources;
    using Narvalo;
    using Narvalo.Web.Semantic;
    using Narvalo.Web.UI.Assets;
    using Serilog;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class OntologyFilterAttribute : ActionFilterAttribute
    {
        //static readonly Lazy<IEnumerable<OpenGraphLocale>> OpenGraphLocales_
        //    = new Lazy<IEnumerable<OpenGraphLocale>>(() =>
        //    {
        //        return from env in ChiffonEnvironmentResolver.Environments
        //               select new OpenGraphLocale(env.UICulture);
        //    });

#if SHOWCASE
        private bool _disabled = true;
#else
        private bool _disabled = false;
#endif

        public OntologyFilterAttribute() { }

        public bool Disabled { get { return _disabled; } set { _disabled = value; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Require.NotNull(filterContext, "filterContext");

            var controller = filterContext.Controller as ChiffonController;
            if (controller == null) {
                throw new InvalidOperationException(
                    "OntologyFilterAttribute should only be applied to ChiffonController.");
            }

            var controllerContext = controller.ChiffonControllerContext;
            var ontology = controllerContext.Ontology;

            if (Disabled) {
                controllerContext.LayoutViewModel.DisableOntology = true;
                ontology.Title = SR.DefaultTitle;
                return;
            }

            // Il semble que "Keywords" soit ignoré par Google, il n'est donc 
            // pas nécessaire de travailler cet aspect là.
            ontology.Keywords = SR.DefaultKeywords;

            ontology.OpenGraph.SiteName = SR.DefaultTitle;

            // Par défaut, on utilise le logo comme image.
            // QUICKFIX: On veut une URL absolue.
            var environment = ChiffonContext.Current.Environment;
            var logoUri = environment.MakeAbsoluteUri(AssetManager.GetImage("logo.png"));

            ontology.OpenGraph.Image = new OpenGraphPng(logoUri) {
                Height = 144,
                Width = 144,
            };

            // Autres langues dans lesquelles la page est disponible.
            //var alternativeLocales = from _ in OpenGraphLocales_.Value
            //                         where _ != ontology.OpenGraph.Locale
            //                         select _;
            //ontology.OpenGraph.AddAlternativeLocales(alternativeLocales);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Require.NotNull(filterContext, "filterContext");

            if (Disabled) {
                return;
            }

            var controller = filterContext.Controller as ChiffonController;
            if (controller == null) {
                throw new InvalidOperationException(
                    "OntologyFilterAttribute should only applied to ChiffonController.");
            }

            var ontology = controller.ChiffonControllerContext.Ontology;

            // Metadata de base.
            if (String.IsNullOrEmpty(ontology.Description)) {
                __Log("No description given, using default.");
                ontology.Description = SR.DefaultDescription;
            }
            if (String.IsNullOrEmpty(ontology.Title)) {
                __Log("No title given, using default.");
                ontology.Title = SR.DefaultTitle;
            }

            // QUICKFIX: On veut une URL absolue.
            ontology.Relationships.HumansTxtUrl
                = new Uri(VirtualPathUtility.ToAbsolute("~/human.txt"), UriKind.Relative);

            __CheckRelationships(ontology.Relationships);
            __CheckOpenGraphMetadata(ontology.OpenGraph);
            __CheckSchemaOrgVocabulary(ontology.SchemaOrg);
        }

        [Conditional("DEBUG")]
        private static void __CheckRelationships(Relationships relationships)
        {
            // Filtre par filterContext.HttpContext.IsDebuggingEnabled ?
            if (relationships.CanonicalUrl == null) {
                __Log("No canonical link given.");
            }
        }

        [Conditional("DEBUG")]
        private static void __CheckOpenGraphMetadata(IOpenGraphMetadata metadata)
        {
            // NB: On sait que metadata.Image n'est pas null car cette propriété 
            // est systématiquement initialisé dans ChiffonController.
            if (metadata.Image.Url == null) {
                __Log("No image URL given.");
            }
        }

        [Conditional("DEBUG")]
        private static void __CheckSchemaOrgVocabulary(SchemaOrgVocabulary vocabulary)
        {
            ;
        }

        [Conditional("DEBUG")]
        private static void __Log(string message)
        {
            Log.Debug(message);
        }
    }
}
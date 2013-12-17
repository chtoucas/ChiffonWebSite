namespace Chiffon.Common.Filters
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Chiffon.Resources;
    using Narvalo;
    using Narvalo.Web.Semantic;
    using Serilog;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class OntologyFilterAttribute : ActionFilterAttribute
    {
        public OntologyFilterAttribute() { }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            var ontology = (Ontology)filterContext.Controller.ViewData["Ontology"];

            // Metadata de base.
            if (String.IsNullOrEmpty(ontology.Description)) {
                __Log("No description given, using default.");
                ontology.Description = SR.DefaultDescription;
            }
            if (String.IsNullOrEmpty(ontology.Title)) {
                __Log("No title given, using default.");
                ontology.Title = SR.DefaultTitle;
            }

            __CheckRelationships(ontology.Relationships);
            __CheckOpenGraphMetadata(ontology.OpenGraph);
            __CheckSchemaOrgVocabulary(ontology.SchemaOrg);
        }

        [Conditional("DEBUG")]
        static void __CheckRelationships(Relationships relationships)
        {
            // Filtre par filterContext.HttpContext.IsDebuggingEnabled ?
            if (relationships.CanonicalUrl == null) {
                __Log("No canonical link given.");
            }
        }

        [Conditional("DEBUG")]
        static void __CheckOpenGraphMetadata(IOpenGraphMetadata metadata)
        {
            // NB: On sait que metadata.Image n'est pas null car cette propriété 
            // est systématiquement initialisé dans PageController.
            if (metadata.Image.Url == null) {
                __Log("No image URL given.");
            }
        }

        [Conditional("DEBUG")]
        static void __CheckSchemaOrgVocabulary(SchemaOrgVocabulary vocabulary)
        {
        }

        [Conditional("DEBUG")]
        static void __Log(string message)
        {
            Log.Debug(message);
        }
    }
}
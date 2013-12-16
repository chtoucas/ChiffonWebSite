namespace Chiffon.Common.Filters
{
    using System;
    using System.Web.Mvc;
    using Narvalo;
    using Narvalo.Web.Semantic;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class SeoPolicyAttribute : ActionFilterAttribute
    {
        string _robotsDirective = "noindex, nofollow";

        public SeoPolicyAttribute() { }

        public string RobotsDirective { get { return _robotsDirective; } set { _robotsDirective = value; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            var ontology = (Ontology)filterContext.Controller.ViewData["Ontology"];
            ontology.RobotsDirective = RobotsDirective;
        }
    }
}
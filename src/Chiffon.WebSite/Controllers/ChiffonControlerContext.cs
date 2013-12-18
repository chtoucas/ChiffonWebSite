namespace Chiffon.Controllers
{
    using Chiffon.Infrastructure;
    using Narvalo.Web.Semantic;

    public class ChiffonControllerContext
    {
        public string ActionName { get; internal set; }
        public string ControllerName { get; internal set; }
        public ChiffonEnvironment Environment { get; internal set; }
        public Ontology Ontology { get; internal set; }
    }
}
namespace Chiffon.Controllers
{
    using Chiffon.Infrastructure;
    using Narvalo.Web.Semantic;

    public class ChiffonControllerContext
    {
        public ChiffonEnvironment Environment { get; internal set; }
        public Ontology Ontology { get; internal set; }
    }
}
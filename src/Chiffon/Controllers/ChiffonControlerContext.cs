namespace Chiffon.Controllers
{
    using Chiffon.Infrastructure;
    using Chiffon.Views;
    using Narvalo.Web.Semantic;

    public sealed class ChiffonControllerContext
    {
        public ChiffonLanguage Language { get; internal set; }

        public LayoutViewModel LayoutViewModel { get; internal set; }

        public Ontology Ontology { get; internal set; }
    }
}
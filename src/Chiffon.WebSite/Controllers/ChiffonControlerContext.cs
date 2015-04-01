namespace Chiffon.Controllers
{
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo.Web.Semantic;

    public class ChiffonControllerContext
    {
        public ChiffonLanguage Language { get; internal set; }

        public LayoutViewModel LayoutViewModel { get; internal set; }

        public Ontology Ontology { get; internal set; }
    }
}
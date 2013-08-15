namespace Chiffon.ViewModels
{
    using System.Linq;

    public class CategoryViewModel : DesignerViewModel
    {
        public PatternItem Pattern { get { return Previews.First(); } }
    }
}
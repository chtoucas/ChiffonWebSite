namespace Chiffon.Data
{
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Fx;

    public interface IViewModelQueries
    {
        IEnumerable<PatternViewItem> GetHomeViewModel();

        Maybe<CategoryViewModel> MayGetCategoryViewModel(
           DesignerKey designerKey, string categoryKey, string languageName);

        DesignerViewModel GetDesignerViewModel(DesignerKey designerKey, string languageName);

        Maybe<CategoryViewModel> MayGetPatternViewModel(
           DesignerKey designerKey,
           string categoryKey,
           string reference,
           string languageName);
    }
}

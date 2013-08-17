namespace Chiffon.Data
{
    using System.Collections.Generic;
    using Chiffon.Data.SqlServer.ViewModelQueries;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo.Fx;

    public class ViewModelQueries : IViewModelQueries
    {
        readonly ChiffonConfig _config;

        public ViewModelQueries(ChiffonConfig config)
        {
            _config = config;
        }

        protected string ConnectionString { get { return _config.SqlConnectionString; } }

        #region IViewModelQueries

        public IEnumerable<PatternViewItem> GetHomeViewModel()
        {
            return new HomeIndexQuery(ConnectionString).Execute();
        }

        public Maybe<CategoryViewModel> MayGetCategoryViewModel(
            DesignerKey designerKey, string categoryKey, string languageName)
        {
            var query = new DesignerCategoryQuery(ConnectionString) {
                CategoryKey = categoryKey,
                DesignerKey = designerKey,
                LanguageName = languageName,
            };
            return query.Execute();
        }

        public DesignerViewModel GetDesignerViewModel(DesignerKey designerKey, string languageName)
        {
            var query = new DesignerIndexQuery(ConnectionString) {
                DesignerKey = designerKey,
                LanguageName = languageName,
            };
            return query.Execute();
        }

        public Maybe<CategoryViewModel> MayGetPatternViewModel(
            DesignerKey designerKey,
            string categoryKey,
            string reference,
            string languageName)
        {
            var query = new DesignerPatternQuery(ConnectionString) { 
                CategoryKey = categoryKey,
                DesignerKey = designerKey,
                LanguageName = languageName,
                Reference = reference,
            };

            return query.Execute();
        }

        #endregion
    }
}

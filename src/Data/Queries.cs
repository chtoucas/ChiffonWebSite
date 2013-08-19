﻿namespace Chiffon.Data
{
    using System.Collections.Generic;
    using Chiffon.Data.SqlServer;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;

    public class Queries : IQueries
    {
        readonly ChiffonConfig _config;

        public Queries(ChiffonConfig config)
        {
            _config = config;
        }

        protected string ConnectionString { get { return _config.SqlConnectionString; } }

        #region IQueries

        public IEnumerable<PatternViewItem> GetHomeViewModel()
        {
            var q = new GetHomeVMQuery(ConnectionString);
            return q.Execute();
        }

        public Designer GetDesigner(DesignerKey designerKey, ChiffonCulture culture)
        {
            var q = new GetDesignerQuery(ConnectionString, designerKey, culture);
            return q.Execute();
        }

        public Pattern GetPattern(DesignerKey designerKey, string reference)
        {
            var q = new GetPatternQuery(ConnectionString, designerKey, reference);
            return q.Execute();
        }

        public IEnumerable<Category> ListCategories(DesignerKey designerKey, ChiffonCulture culture)
        {
            var q = new ListCategoriesQuery(ConnectionString, designerKey, culture);
            return q.Execute();
        }

        public IEnumerable<Designer> ListDesigners(ChiffonCulture culture)
        {
            var q = new ListDesignersQuery(ConnectionString, culture);
            return q.Execute();
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey)
        {
            var q = new ListPatternsQuery(ConnectionString, designerKey);
            return q.Execute();
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey)
        {
            var q = new ListPatternsQuery(ConnectionString, designerKey) { CategoryKey = categoryKey };
            return q.Execute();
        }

        #endregion
    }
}

namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using Chiffon.Data.SqlServer;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;

    public class Queries : IQueries
    {
        readonly ChiffonConfig _config;

        public Queries(ChiffonConfig config)
        {
            _config = config;
        }

        protected string ConnectionString { get { return _config.SqlConnectionString; } }

        #region IQueries

        public IEnumerable<Pattern> ListShowcasedPatterns()
        {
            var q = new ListShowcasedPatternsQuery(ConnectionString);
            return q.Execute();
        }

        public Designer GetDesigner(DesignerKey designerKey, CultureInfo culture)
        {
            var q = new GetDesignerQuery(ConnectionString, designerKey, culture);
            return q.Execute();
        }

        public Pattern GetPattern(DesignerKey designerKey, string reference, string version)
        {
            var q = new GetPatternQuery(ConnectionString, designerKey, reference, version);
            return q.Execute();
        }

        public IEnumerable<Category> ListCategories(DesignerKey designerKey)
        {
            var q = new ListCategoriesQuery(ConnectionString, designerKey);
            return q.Execute();
        }

        public IEnumerable<Designer> ListDesigners(CultureInfo culture)
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

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, string categoryKey, bool published)
        {
            var q = new ListPatternsQuery(ConnectionString, designerKey) {
                CategoryKey = categoryKey,
                Published = published,
            };
            return q.Execute();
        }

        public IEnumerable<Pattern> ListPatterns(DesignerKey designerKey, bool published)
        {
            var q = new ListPatternsQuery(ConnectionString, designerKey) { Published = published };
            return q.Execute();
        }

        #endregion
    }
}

namespace Chiffon.Persistence.SqlServer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Data;

    public class ListCategoriesQuery : StoredProcedure<IEnumerable<Category>>
    {
        public ListCategoriesQuery(string connectionString, DesignerKey designerKey)
            : base(connectionString, "usp_ListCategories")
        {
            DesignerKey = designerKey;
        }

        public DesignerKey DesignerKey { get; private set; }

        protected override IEnumerable<Category> Execute(SqlDataReader rdr)
        {
            Require.NotNull(rdr, "rdr");

            var categories = new List<Category>();

            // Catégories du designer (avec au moins un motif).
            while (rdr.Read()) {
                var category = new Category(DesignerKey, rdr.GetString("category")) {
                    DisplayName = rdr.GetString("display_name"),
                    PatternsCount = rdr.GetInt32("patterns_count"),
                };
                categories.Add(category);
            }

            return categories;
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            parameters.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
        }
    }
}
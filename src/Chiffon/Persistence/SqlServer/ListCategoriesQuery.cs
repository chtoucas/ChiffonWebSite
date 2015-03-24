namespace Chiffon.Persistence.SqlServer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using Chiffon.Entities;
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

        protected override IEnumerable<Category> Execute(SqlDataReader reader)
        {
            Require.NotNull(reader, "reader");

            var categories = new List<Category>();

            // Catégories du designer (avec au moins un motif).
            while (reader.Read())
            {
                var category = new Category(DesignerKey, reader.GetStringUnsafe("category")) {
                    DisplayName = reader.GetStringUnsafe("display_name"),
                    PatternsCount = reader.GetInt32Unsafe("patterns_count"),
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
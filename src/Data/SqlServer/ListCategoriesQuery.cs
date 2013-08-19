namespace Chiffon.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo.Data;

    public class ListCategoriesQuery : StoredProcedure<IEnumerable<Category>>
    {
        public ListCategoriesQuery(string connectionString, DesignerKey designerKey, ChiffonCulture culture)
            : base(connectionString, "usp_ListCategories")
        {
            DesignerKey = designerKey;
            Culture = culture;
        }

        public DesignerKey DesignerKey { get; private set; }
        public ChiffonCulture Culture { get; private set; }

        protected override IEnumerable<Category> Execute(SqlDataReader rdr)
        {
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

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@language", SqlDbType.Char, Culture.LanguageName);
        }
    }
}
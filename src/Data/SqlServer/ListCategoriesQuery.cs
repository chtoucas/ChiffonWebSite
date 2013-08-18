namespace Chiffon.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Narvalo.Data;

    public class ListCategoriesQuery : StoredProcedure<IEnumerable<Category>>
    {
        public ListCategoriesQuery(string connectionString, DesignerKey designerKey, string languageName)
            : base(connectionString, "usp_ListCategories")
        {
            DesignerKey = designerKey;
            LanguageName = languageName;
        }

        public DesignerKey DesignerKey { get; private set; }
        public string LanguageName { get; private set; }

        protected override IEnumerable<Category> Execute(SqlDataReader rdr)
        {
            var categories = new List<Category>();

            // Catégories du designer (avec au moins un motif).
            while (rdr.Read()) {
                var category = new Category(DesignerKey, rdr.GetString("category")) {
                    DisplayName = rdr.GetString("display_name"),
                    PatternCount = rdr.GetInt32("pattern_count"),
                };
                categories.Add(category);
            }

            return categories;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@language", SqlDbType.Char, LanguageName);
        }
    }
}
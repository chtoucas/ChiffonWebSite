namespace Chiffon.Data.SqlServer.ViewModelQueries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;
    using Narvalo.Fx;

    public class DesignerPatternQuery : StoredProcedure<Maybe<CategoryViewModel>>
    {
        public DesignerPatternQuery(string connectionString)
            : base(connectionString, "usp_GetPatternVm") { }

        public string CategoryKey { get; set; }
        public DesignerKey DesignerKey { get; set; }
        public string LanguageName { get; set; }
        public string Reference { get; set; }

        public override Maybe<CategoryViewModel> Execute()
        {
            var result = new CategoryViewModel();

            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = CreateCommand(cnx)) {
                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        // Informations sur le designer.
                        if (rdr.Read()) {
                            result.Designer = rdr.GetDesignerItem(DesignerKey);
                        }

                        rdr.NextResult();

                        // Catégories du designer (avec au moins un motif).
                        var categories = new List<CategoryViewItem>();
                        while (rdr.Read()) {
                            categories.Add(rdr.GetCategoryItem());
                        }
                        result.Categories = categories;

                        rdr.NextResult();

                        // Les motifs dans la catégorie avec le motif sélectionné en premier.
                        var patterns = new List<PatternViewItem>();

                        if (!rdr.Read()) {
                            return Maybe<CategoryViewModel>.None;
                        }

                        patterns.Add(new PatternViewItem {
                            CategoryKey = CategoryKey,
                            DesignerKey = DesignerKey,
                            DesignerName = result.Designer.DisplayName,
                            Reference = Reference
                        });

                        rdr.NextResult();

                        while (rdr.Read()) {
                            patterns.Add(rdr.GetPatternItem(DesignerKey, CategoryKey, result.Designer.DisplayName));
                        }
                        result.Patterns = patterns;
                    }
                }
            }

            return Maybe.Create(result);
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@category", SqlDbType.NVarChar, CategoryKey);
            command.AddParameter("@reference", SqlDbType.NVarChar, Reference);
            command.AddParameter("@language", SqlDbType.Char, LanguageName);
        }
    }
}
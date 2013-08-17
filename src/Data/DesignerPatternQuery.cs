namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Fx;

    public class DesignerPatternQuery : BaseQuery
    {
        public DesignerPatternQuery(string connectionString) : base(connectionString) { }

        public Maybe<CategoryViewModel> Execute(
            DesignerKey designerKey,
            string categoryKey,
            string reference,
            string languageName)
        {
            var model = new CategoryViewModel();

            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = NewStoredProcedure("usp_fo_getPattern", cnx)) {
                    cmd.AddParameter("@designer", SqlDbType.NVarChar, designerKey.Value);
                    cmd.AddParameter("@category", SqlDbType.NVarChar, categoryKey);
                    cmd.AddParameter("@reference", SqlDbType.NVarChar, reference);
                    cmd.AddParameter("@language", SqlDbType.Char, languageName);

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        // Informations sur le designer.
                        if (rdr.Read()) {
                            model.Designer = rdr.GetDesigner(designerKey);
                        }

                        rdr.NextResult();

                        // Catégories du designer (avec au moins un motif).
                        var categories = new List<CategoryItem>();
                        while (rdr.Read()) {
                            categories.Add(rdr.GetCategory());
                        }
                        model.Categories = categories;

                        rdr.NextResult();

                        // Les motifs dans la catégorie avec le motif sélectionné en premier.
                        var patterns = new List<PatternItem>();

                        if (!rdr.Read()) {
                            return Maybe<CategoryViewModel>.None;
                        }

                        patterns.Add(new PatternItem {
                            CategoryKey = categoryKey,
                            DesignerKey = designerKey,
                            DesignerName = model.Designer.DisplayName,
                            Reference = reference
                        });

                        rdr.NextResult();

                        while (rdr.Read()) {
                            patterns.Add(rdr.GetPattern(designerKey, categoryKey, model.Designer.DisplayName));
                        }
                        model.Patterns = patterns;
                    }
                }
            }

            return Maybe.Create(model);
        }
    }
}
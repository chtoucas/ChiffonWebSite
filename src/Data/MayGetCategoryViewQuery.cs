namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Fx;

    public class MayGetCategoryViewQuery
    {
        readonly SqlHelper _sqlHelper;

        public MayGetCategoryViewQuery(SqlHelper sqlHelper)
        {
            Requires.NotNull(sqlHelper, "sqlHelper");

            _sqlHelper = sqlHelper;
        }

        public Maybe<CategoryViewModel> Execute(DesignerKey designerKey, string categoryKey, string languageName)
        {
            var model = new CategoryViewModel();

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = SqlHelper.StoredProcedure("usp_fo_getCategory", cnx)) {
                    cmd.AddParameter("@designer", SqlDbType.NVarChar, designerKey.Value);
                    cmd.AddParameter("@category", SqlDbType.NVarChar, categoryKey);
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

                        // Les motifs de la catégorie.
                        var patterns = new List<PatternItem>();
                        while (rdr.Read()) {
                            patterns.Add(rdr.GetPattern(designerKey, categoryKey, model.Designer.DisplayName));
                        }
                        model.Patterns = patterns;
                    }
                }
            }

            return model.Patterns.Count() > 0 ? Maybe.Create(model) : Maybe<CategoryViewModel>.None;
        }

        public Maybe<CategoryViewModel> Execute(
            DesignerKey designerKey,
            string categoryKey,
            string reference,
            string languageName)
        {
            var model = new CategoryViewModel();

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = SqlHelper.StoredProcedure("usp_fo_getPattern", cnx)) {
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
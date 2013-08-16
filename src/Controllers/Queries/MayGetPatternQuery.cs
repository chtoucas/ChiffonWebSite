namespace Chiffon.Controllers.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Fx;

    public class MayGetPatternQuery
    {
        readonly SqlHelper _sqlHelper;

        public MayGetPatternQuery(SqlHelper sqlHelper)
        {
            Requires.NotNull(sqlHelper, "sqlHelper");

            _sqlHelper = sqlHelper;
        }

        public Maybe<CategoryViewModel> Execute(
            DesignerKey designerKey,
            string categoryKey,
            string reference,
            string languageName)
        {

            var model = new CategoryViewModel();

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = SqlHelper.CreateStoredProcedure("usp_fo_getPattern", cnx)) {
                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designerKey.Value;
                    p.Add("@category", SqlDbType.NVarChar).Value = categoryKey;
                    p.Add("@reference", SqlDbType.NVarChar).Value = reference;
                    p.Add("@language", SqlDbType.Char).Value = languageName;

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
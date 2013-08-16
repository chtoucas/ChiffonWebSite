namespace Chiffon.Controllers.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Fx;

    public class MayGetCategoryQuery
    {
        readonly SqlHelper _sqlHelper;

        public MayGetCategoryQuery(SqlHelper sqlHelper)
        {
            Requires.NotNull(sqlHelper, "sqlHelper");

            _sqlHelper = sqlHelper;
        }

        public Maybe<CategoryViewModel> Execute(DesignerKey designerKey, string categoryKey, string languageName)
        {
            var model = new CategoryViewModel();

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = SqlHelper.CreateStoredProcedure("usp_fo_getCategory", cnx)) {
                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designerKey.Value;
                    p.Add("@category", SqlDbType.NVarChar).Value = categoryKey;
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
    }
}
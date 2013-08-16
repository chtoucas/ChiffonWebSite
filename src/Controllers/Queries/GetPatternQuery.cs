namespace Chiffon.Controllers.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Data;

    public class GetPatternQuery
    {
        readonly DbHelper _dbHelper;

        public GetPatternQuery(DbHelper dbHelper)
        {
            Requires.NotNull(dbHelper, "dbHelper");

            _dbHelper = dbHelper;
        }

        public PatternViewModel Execute(DesignerKey designer, string reference, string language)
        {

            var model = new PatternViewModel();

            using (var cnx = _dbHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_fo_getPattern";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@reference", SqlDbType.NVarChar).Value = reference;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designer.Key;
                    p.Add("@language", SqlDbType.Char).Value = language;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        // Informations sur le designer.
                        model.Designer = rdr.GetDesigner(designer);

                        // Catégories du designer (avec au moins un motif).
                        rdr.NextResult();
                        model.Categories = rdr.GetCategories();

                        // Détails sur le motif.
                        rdr.NextResult();
                        if (rdr.Read()) {
                            model.Pattern = rdr.GetPattern(designer, model.Designer.DisplayName);
                        }

                        // Autres motifs dans la même catégorie.
                        rdr.NextResult();
                        model.Previews = rdr.GetPatterns(designer, model.Designer.DisplayName);
                    }
                }
            }

            return model;
        }
    }
}
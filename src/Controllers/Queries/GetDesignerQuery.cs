namespace Chiffon.Controllers.Queries
{
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;

    public class GetDesignerQuery
    {
        readonly DbHelper _dbHelper;

        public GetDesignerQuery(DbHelper dbHelper)
        {
            Requires.NotNull(dbHelper, "dbHelper");

            _dbHelper = dbHelper;
        }

        public DesignerViewModel Execute(DesignerKey designer, string language)
        {
            var model = new DesignerViewModel();

            using (var cnx = _dbHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_fo_getDesigner";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designer.Key;
                    p.Add("@language", SqlDbType.Char).Value = language;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        // Informations sur le designer.
                        model.Designer = rdr.GetDesigner(designer);

                        // Catégories du designer (avec au moins un motif).
                        rdr.NextResult();
                        model.Categories = rdr.GetCategories();

                        // Les motifs du designer.
                        rdr.NextResult();
                        model.Previews = rdr.GetPatterns(designer, model.Designer.DisplayName);
                    }
                }
            }

            return model;
        }
    }
}
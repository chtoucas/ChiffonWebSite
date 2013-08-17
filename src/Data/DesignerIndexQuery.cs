namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;

    public class DesignerIndexQuery : BaseQuery
    {
        public DesignerIndexQuery(string connectionString) : base(connectionString) { }

        public DesignerViewModel Execute(DesignerKey designerKey, string languageName)
        {
            var model = new DesignerViewModel();

            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = NewStoredProcedure("usp_fo_getDesigner", cnx)) {
                    cmd.AddParameter("@designer", SqlDbType.NVarChar, designerKey.Value);
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

                        // Les motifs du designer.
                        var previews = new List<PatternItem>();
                        while (rdr.Read()) {
                            previews.Add(rdr.GetPattern(designerKey, model.Designer.DisplayName));
                        }
                        model.Previews = previews;
                    }
                }
            }

            return model;
        }
    }
}
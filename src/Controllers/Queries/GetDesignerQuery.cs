﻿namespace Chiffon.Controllers.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;

    public class GetDesignerQuery
    {
        readonly SqlHelper _sqlHelper;

        public GetDesignerQuery(SqlHelper sqlHelper)
        {
            Requires.NotNull(sqlHelper, "sqlHelper");

            _sqlHelper = sqlHelper;
        }

        public DesignerViewModel Execute(DesignerKey designerKey, string languageName)
        {
            var model = new DesignerViewModel();

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = SqlHelper.CreateStoredProcedure("usp_fo_getDesigner", cnx)) {
                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designerKey.Value;
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
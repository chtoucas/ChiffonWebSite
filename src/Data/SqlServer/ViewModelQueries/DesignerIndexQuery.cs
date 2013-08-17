namespace Chiffon.Data.SqlServer.ViewModelQueries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;

    public class DesignerIndexQuery : StoredProcedure<DesignerViewModel>
    {
        public DesignerIndexQuery(string connectionString) 
            : base(connectionString, "usp_GetDesignerVm") { }

        public DesignerKey DesignerKey { get; set; }
        public string LanguageName { get; set; }

        public override DesignerViewModel Execute()
        {
            var result = new DesignerViewModel();

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

                        // Les motifs du designer.
                        var previews = new List<PatternViewItem>();
                        while (rdr.Read()) {
                            previews.Add(rdr.GetPatternItem(DesignerKey, result.Designer.DisplayName));
                        }
                        result.Previews = previews;
                    }
                }
            }

            return result;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@language", SqlDbType.Char, LanguageName);
        }
    }
}
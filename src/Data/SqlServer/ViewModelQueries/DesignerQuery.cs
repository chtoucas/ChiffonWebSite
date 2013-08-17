namespace Chiffon.Data.SqlServer.ViewModelQueries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;

    public class DesignerQuery : StoredProcedure<DesignerViewModel>
    {
        public DesignerQuery(string connectionString)
            : base(connectionString, "usp_GetCommonDesignerVm") { }

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
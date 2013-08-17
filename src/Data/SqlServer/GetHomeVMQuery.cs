namespace Chiffon.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;

    public class GetHomeVMQuery : StoredProcedure<IEnumerable<PatternViewItem>>
    {
        public GetHomeVMQuery(string connectionString)
            : base(connectionString, "usp_GetHomeVm") { }

        public override IEnumerable<PatternViewItem> Execute()
        {
            var result = new List<PatternViewItem>();

            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = CreateCommand(cnx)) {
                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (rdr.Read()) {
                            var pattern = new PatternViewItem {
                                CategoryKey = rdr.GetString("category"),
                                DesignerKey = DesignerKey.Parse(rdr.GetString("designer")),
                                DesignerName = rdr.GetString("designer_name"),
                                Reference = rdr.GetString("reference"),
                            };
                            result.Add(pattern);
                        }
                    }
                }
            }

            return result;
        }
    }
}
namespace Chiffon.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;

    public class GetHomeVMQuery : StoredProcedure<IEnumerable<PatternViewItem>>
    {
        public GetHomeVMQuery(string connectionString)
            : base(connectionString, "usp_GetHomeVm") { }

        protected override IEnumerable<PatternViewItem> Execute(SqlDataReader rdr)
        {
            var result = new List<PatternViewItem>();

            while (rdr.Read()) {
                var pattern = new PatternViewItem {
                    CategoryKey = rdr.GetString("category"),
                    DesignerKey = DesignerKey.Parse(rdr.GetString("designer")),
                    DesignerName = rdr.GetString("designer_name"),
                    Reference = rdr.GetString("reference"),
                };
                result.Add(pattern);
            }

            return result;
        }
    }
}
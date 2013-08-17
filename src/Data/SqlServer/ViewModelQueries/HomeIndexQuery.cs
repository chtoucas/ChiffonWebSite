namespace Chiffon.Data.SqlServer.ViewModelQueries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.ViewModels;

    public class HomeIndexQuery : StoredProcedure<IEnumerable<PatternViewItem>>
    {
        public HomeIndexQuery(string connectionString)
            : base(connectionString, "usp_GetShowcasedPatterns") { }

        public override IEnumerable<PatternViewItem> Execute()
        {
            var result = new List<PatternViewItem>();

            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = CreateCommand(cnx)) {
                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (rdr.Read()) {
                            result.Add(rdr.GetPatternItem());
                        }
                    }
                }
            }

            return result;
        }
    }
}
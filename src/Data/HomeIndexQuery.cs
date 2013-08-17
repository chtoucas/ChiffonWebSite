namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.ViewModels;

    public class HomeIndexQuery : BaseQuery
    {
        public HomeIndexQuery(string connectionString) : base(connectionString) { }

        public List<PatternItem> Execute()
        {
            var model = new List<PatternItem>();

            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = NewStoredProcedure("usp_fo_getShowcasedPatterns", cnx)) {
                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (rdr.Read()) {
                            model.Add(rdr.GetPattern());
                        }
                    }
                }
            }

            return model;
        }
    }
}
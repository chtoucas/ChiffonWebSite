namespace Chiffon.Controllers.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;

    public class ListShowcasedPatternsQuery
    {
        readonly SqlHelper _sqlHelper;

        public ListShowcasedPatternsQuery(SqlHelper sqlHelper)
        {
            Requires.NotNull(sqlHelper, "sqlHelper");

            _sqlHelper = sqlHelper;
        }

        public List<PatternItem> Execute()
        {
            var model = new List<PatternItem>();

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = SqlHelper.CreateStoredProcedure("usp_fo_getShowcasedPatterns", cnx)) {
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
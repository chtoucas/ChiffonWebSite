namespace Chiffon.Data
{
    using System.Collections.Generic;
    using System.Data;
    using Chiffon.ViewModels;
    using Narvalo;

    public class ListShowcasedPatternsViewQuery
    {
        readonly SqlHelper _sqlHelper;

        public ListShowcasedPatternsViewQuery(SqlHelper sqlHelper)
        {
            Requires.NotNull(sqlHelper, "sqlHelper");

            _sqlHelper = sqlHelper;
        }

        public List<PatternItem> Execute()
        {
            var model = new List<PatternItem>();

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = SqlHelper.StoredProcedure("usp_fo_getShowcasedPatterns", cnx)) {
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
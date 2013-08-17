namespace Chiffon.Data
{
    using System.Data;
    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;
    using Narvalo.Fx;

    public class MayGetPatternQuery
    {
        readonly SqlHelper _sqlHelper;

        public MayGetPatternQuery(SqlHelper sqlHelper)
        {
            Requires.NotNull(sqlHelper, "sqlHelper");
            _sqlHelper = sqlHelper;
        }

        public Maybe<Pattern> Execute(DesignerKey designerKey, string reference)
        {
            var result = Maybe<Pattern>.None;

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = SqlHelper.StoredProcedure("usp_getPattern", cnx)) {
                    cmd.AddParameter("@reference", SqlDbType.NVarChar, reference);
                    cmd.AddParameter("@designer", SqlDbType.NVarChar, designerKey.Value);

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (rdr.Read()) {
                            var pattern = new Pattern(new PatternId(designerKey, reference)) {
                                //CreationTime = rdr.GetDateTimeColumn("creation_time"),
                                Preferred = rdr.GetBooleanColumn("preferred"),
                                Published = rdr.GetBooleanColumn("online"),
                                Showcased = rdr.GetBooleanColumn("showcased"),
                            };

                            result = Maybe.Create(pattern);
                        }
                    }
                }
            }

            return result;
        }
    }
}
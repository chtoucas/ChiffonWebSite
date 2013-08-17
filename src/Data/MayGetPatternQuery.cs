namespace Chiffon.Data
{
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Narvalo.Data;
    using Narvalo.Fx;

    public class MayGetPatternQuery : BaseQuery
    {
        public MayGetPatternQuery(string connectionString) : base(connectionString) { }

        public Maybe<Pattern> Execute(DesignerKey designerKey, string reference)
        {
            var result = Maybe<Pattern>.None;

            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = NewStoredProcedure("usp_getPattern", cnx)) {
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
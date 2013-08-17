namespace Chiffon.Data.SqlServer.EntityQueries
{
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Narvalo.Data;
    using Narvalo.Fx;

    public class MayGetPatternQuery : StoredProcedure<Maybe<Pattern>>
    {
        public MayGetPatternQuery(string connectionString)
            : base(connectionString, "usp_GetPattern") { }

        public DesignerKey DesignerKey { get; set; }
        public string Reference { get; set; }

        public override Maybe<Pattern> Execute()
        {
            var result = Maybe<Pattern>.None;

            using (var connection = new SqlConnection(ConnectionString)) {
                using (var cmd = CreateCommand(connection)) {
                    connection.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (rdr.Read()) {
                            var pattern = new Pattern(new PatternId(DesignerKey, Reference)) {
                                //CreationTime = rdr.GetDateTimeColumn("creation_time"),
                                Preferred = rdr.GetBoolean("preferred"),
                                Published = rdr.GetBoolean("online"),
                                Showcased = rdr.GetBoolean("showcased"),
                            };

                            result = Maybe.Create(pattern);
                        }
                    }
                }
            }

            return result;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@reference", SqlDbType.NVarChar, Reference);
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
        }
    }
}
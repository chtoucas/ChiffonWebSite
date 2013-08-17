namespace Chiffon.Data.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Narvalo.Data;

    public class GetPatternQuery : StoredProcedure<Pattern>
    {
        public GetPatternQuery(string connectionString, DesignerKey designerKey, string reference)
            : base(connectionString, "usp_GetPattern")
        {
            DesignerKey = designerKey;
            Reference = reference;
        }

        public DesignerKey DesignerKey { get; private set; }
        public string Reference { get; private set; }

        public override Pattern Execute()
        {
            Pattern result = null;

            using (var connection = new SqlConnection(ConnectionString)) {
                using (var cmd = CreateCommand(connection)) {
                    connection.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (rdr.Read()) {
                            var pattern = new Pattern(new PatternId(DesignerKey, Reference)) {
                                CategoryKey = rdr.GetString("category"),
                                CreationTime = rdr.GetDateTime("creation_time"),
                                Preferred = rdr.GetBoolean("preferred"),
                                Published = rdr.GetBoolean("published"),
                                Showcased = rdr.GetBoolean("showcased"),
                            };

                            result = pattern;
                        }
                    }
                }
            }

            return result;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@reference", SqlDbType.NVarChar, Reference);
        }
    }
}
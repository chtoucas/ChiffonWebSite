namespace Chiffon.Persistence.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public class GetPatternQuery : StoredProcedure<Pattern>
    {
        public GetPatternQuery(string connectionString, DesignerKey designerKey, string reference, string variant)
            : base(connectionString, "usp_GetPattern")
        {
            DesignerKey = designerKey;
            Reference = reference;
            Variant = variant;

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        public DesignerKey DesignerKey { get; private set; }
        public string Reference { get; private set; }
        public string Variant { get; private set; }

        protected override Pattern Execute(SqlDataReader rdr)
        {
            Require.NotNull(rdr, "rdr");

            if (!rdr.Read()) { return null; }

            return new Pattern(new PatternId(DesignerKey, Reference), Variant) {
                CategoryKey = rdr.GetString("category"),
                CreationTime = rdr.GetDateTime("creation_time"),
                HasPreview = rdr.GetBoolean("preview"),
                LastModifiedTime = rdr.GetDateTime("last_modified_time"),
                Preferred = rdr.GetBoolean("preferred"),
                Published = rdr.GetBoolean("published"),
                Showcased = rdr.GetBoolean("showcased"),
            };
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@reference", SqlDbType.NVarChar, Reference);
            command.AddParameter("@version", SqlDbType.NVarChar, Variant);
        }
    }
}
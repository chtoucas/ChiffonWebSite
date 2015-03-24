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

        protected override Pattern Execute(SqlDataReader reader)
        {
            Require.NotNull(reader, "reader");

            if (!reader.Read()) { return null; }

            return new Pattern(new PatternId(DesignerKey, Reference), Variant) {
                CategoryKey = reader.GetString("category"),
                CreationTime = reader.GetDateTime("creation_time"),
                HasPreview = reader.GetBoolean("preview"),
                LastModifiedTime = reader.GetDateTime("last_modified_time"),
                Preferred = reader.GetBoolean("preferred"),
                Published = reader.GetBoolean("published"),
                Showcased = reader.GetBoolean("showcased"),
            };
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            parameters.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            parameters.AddParameter("@reference", SqlDbType.NVarChar, Reference);
            parameters.AddParameter("@version", SqlDbType.NVarChar, Variant);
        }
    }
}
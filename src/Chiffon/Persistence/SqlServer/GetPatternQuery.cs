namespace Chiffon.Persistence.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;

    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public sealed class GetPatternQuery : StoredProcedure<Pattern>
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
                CategoryKey = reader.GetStringUnsafe("category"),
                CreationTime = reader.GetDateTimeUnsafe("creation_time"),
                HasPreview = reader.GetBooleanUnsafe("preview"),
                LastModifiedTime = reader.GetDateTimeUnsafe("last_modified_time"),
                Preferred = reader.GetBooleanUnsafe("preferred"),
                Published = reader.GetBooleanUnsafe("published"),
                Showcased = reader.GetBooleanUnsafe("showcased"),
            };
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Require.NotNull(parameters, "parameters");

            parameters.AddParameterUnsafe("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            parameters.AddParameterUnsafe("@reference", SqlDbType.NVarChar, Reference);
            parameters.AddParameterUnsafe("@version", SqlDbType.NVarChar, Variant);
        }
    }
}
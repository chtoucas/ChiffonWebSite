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

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        public DesignerKey DesignerKey { get; private set; }
        public string Reference { get; private set; }

        protected override Pattern Execute(SqlDataReader rdr)
        {
            if (!rdr.Read()) { return null; }

            var result = new Pattern(new PatternId(DesignerKey, Reference)) {
                CategoryKey = rdr.GetString("category"),
                CreationTime = rdr.GetDateTime("creation_time"),
                Preferred = rdr.GetBoolean("preferred"),
                Published = rdr.GetBoolean("published"),
                Showcased = rdr.GetBoolean("showcased"),
            };

            return result;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@reference", SqlDbType.NVarChar, Reference);
        }
    }
}
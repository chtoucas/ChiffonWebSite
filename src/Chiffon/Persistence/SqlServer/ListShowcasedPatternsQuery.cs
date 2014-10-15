namespace Chiffon.Persistence.SqlServer
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public class ListShowcasedPatternsQuery : StoredProcedure<IEnumerable<Pattern>>
    {
        public ListShowcasedPatternsQuery(string connectionString)
            : base(connectionString, "usp_ListShowcasedPatterns") { }

        protected override IEnumerable<Pattern> Execute(SqlDataReader rdr)
        {
            Require.NotNull(rdr, "rdr");

            var result = new List<Pattern>();

            while (rdr.Read()) {
                var designerKey = DesignerKey.Parse(rdr.GetString("designer"));
                var patternId = new PatternId(designerKey, rdr.GetString("reference"));
                var version = rdr.GetString("version");
                var pattern = new Pattern(patternId, version) {
                    CategoryKey = rdr.GetString("category"),
                    CreationTime = rdr.GetDateTime("creation_time"),
                    HasPreview = rdr.GetBoolean("preview"),
                    LastModifiedTime = rdr.GetDateTime("last_modified_time"),
                    Preferred = true,
                    Published = true,
                    Showcased = true,
                };
                result.Add(pattern);
            }

            return result;
        }
    }
}
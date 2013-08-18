namespace Chiffon.Data.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Narvalo.Data;

    public class ListPatternsQuery : StoredProcedure<IEnumerable<Pattern>>
    {
        public ListPatternsQuery(string connectionString, DesignerKey designerKey)
            : base(connectionString, "usp_ListPatterns")
        {
            DesignerKey = designerKey;
        }

        public string CategoryKey { get; set; }
        public DesignerKey DesignerKey { get; private set; }

        protected override IEnumerable<Pattern> Execute(SqlDataReader rdr)
        {
            var patterns = new List<Pattern>();

            while (rdr.Read()) {
                var patternId = new PatternId(DesignerKey, rdr.GetString("reference"));
                var pattern = new Pattern(patternId) {
                    CategoryKey = rdr.GetString("category"),
                    CreationTime = rdr.GetDateTime("creation_time"),
                    Preferred = rdr.GetBoolean("preferred"),
                    Published = true,
                    Showcased = rdr.GetBoolean("showcased"),
                };
                patterns.Add(pattern);
            }

            return patterns;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);

            if (!String.IsNullOrEmpty(CategoryKey)) {
                command.AddParameter("@category", SqlDbType.NVarChar, CategoryKey);
            }
            else {
                command.AddParameter("@category", SqlDbType.NVarChar, DBNull.Value);
            }
        }
    }
}
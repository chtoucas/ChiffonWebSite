namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public class ListPatternsQuery : StoredProcedure<IEnumerable<Pattern>>
    {
        bool? _published = true;

        public ListPatternsQuery(string connectionString, DesignerKey designerKey)
            : base(connectionString, "usp_ListPatterns")
        {
            DesignerKey = designerKey;
        }

        public string CategoryKey { get; set; }
        public DesignerKey DesignerKey { get; private set; }
        public bool? Published { get { return _published; } set { _published = value; } }

        protected override IEnumerable<Pattern> Execute(SqlDataReader rdr)
        {
            Require.NotNull(rdr, "rdr");

            var patterns = new List<Pattern>();

            while (rdr.Read()) {
                var patternId = new PatternId(DesignerKey, rdr.GetString("reference"));
                var version = rdr.GetString("version");

                var pattern = new Pattern(patternId, version) {
                    CategoryKey = rdr.GetString("category"),
                    CreationTime = rdr.GetDateTime("creation_time"),
                    HasPreview = rdr.GetBoolean("preview"),
                    LastModifiedTime = rdr.GetDateTime("last_modified_time"),
                    Preferred = rdr.GetBoolean("preferred"),
                    Published = rdr.GetBoolean("published"),
                    Showcased = rdr.GetBoolean("showcased"),
                };

                patterns.Add(pattern);
            }

            return patterns;
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            parameters.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);

            if (Published.HasValue) {
                parameters.AddParameter("@published", SqlDbType.Bit, Published.Value);
            }
            else {
                parameters.AddParameter("@published", SqlDbType.Bit, DBNull.Value);
            }

            if (!String.IsNullOrEmpty(CategoryKey)) {
                parameters.AddParameter("@category", SqlDbType.NVarChar, CategoryKey);
            }
            else {
                parameters.AddParameter("@category", SqlDbType.NVarChar, DBNull.Value);
            }
        }
    }
}
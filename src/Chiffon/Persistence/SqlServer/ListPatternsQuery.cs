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

        protected override IEnumerable<Pattern> Execute(SqlDataReader reader)
        {
            Require.NotNull(reader, "reader");

            var patterns = new List<Pattern>();

            while (reader.Read())
            {
                var patternId = new PatternId(DesignerKey, reader.GetString("reference"));
                var version = reader.GetString("version");

                var pattern = new Pattern(patternId, version) {
                    CategoryKey = reader.GetString("category"),
                    CreationTime = reader.GetDateTime("creation_time"),
                    HasPreview = reader.GetBoolean("preview"),
                    LastModifiedTime = reader.GetDateTime("last_modified_time"),
                    Preferred = reader.GetBoolean("preferred"),
                    Published = reader.GetBoolean("published"),
                    Showcased = reader.GetBoolean("showcased"),
                };

                patterns.Add(pattern);
            }

            return patterns;
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            parameters.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);

            if (Published.HasValue)
            {
                parameters.AddParameter("@published", SqlDbType.Bit, Published.Value);
            }
            else
            {
                parameters.AddParameter("@published", SqlDbType.Bit, DBNull.Value);
            }

            if (!String.IsNullOrEmpty(CategoryKey))
            {
                parameters.AddParameter("@category", SqlDbType.NVarChar, CategoryKey);
            }
            else
            {
                parameters.AddParameter("@category", SqlDbType.NVarChar, DBNull.Value);
            }
        }
    }
}
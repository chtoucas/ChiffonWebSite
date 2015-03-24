namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public sealed class ListPatternsQuery : StoredProcedure<IEnumerable<Pattern>>
    {
        private bool? _published = true;

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
                var patternId = new PatternId(DesignerKey, reader.GetStringUnsafe("reference"));
                var version = reader.GetStringUnsafe("version");

                var pattern = new Pattern(patternId, version) {
                    CategoryKey = reader.GetStringUnsafe("category"),
                    CreationTime = reader.GetDateTimeUnsafe("creation_time"),
                    HasPreview = reader.GetBooleanUnsafe("preview"),
                    LastModifiedTime = reader.GetDateTimeUnsafe("last_modified_time"),
                    Preferred = reader.GetBooleanUnsafe("preferred"),
                    Published = reader.GetBooleanUnsafe("published"),
                    Showcased = reader.GetBooleanUnsafe("showcased"),
                };

                patterns.Add(pattern);
            }

            return patterns;
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Require.NotNull(parameters, "parameters");

            parameters.AddParameterUnsafe("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            parameters.AddParameterOrNullUnsafe("@published", SqlDbType.Bit, Published);
            parameters.AddParameterOrNullUnsafe("@category", SqlDbType.NVarChar, CategoryKey, !String.IsNullOrEmpty(CategoryKey));
        }
    }
}
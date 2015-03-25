namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public sealed class ListShowcasedPatternsQuery : StoredProcedure<IEnumerable<Pattern>>
    {
        public ListShowcasedPatternsQuery(string connectionString)
            : base(connectionString, "usp_ListShowcasedPatterns")
        {
            Contract.Requires(!String.IsNullOrEmpty(connectionString));
        }

        protected override IEnumerable<Pattern> Execute(SqlDataReader reader)
        {
            Check.NotNull(reader, "The base class guarantees that the parameter is not null.");

            var result = new List<Pattern>();

            while (reader.Read())
            {
                var designerKey = DesignerKey.Parse(reader.GetStringUnsafe("designer"));
                var patternId = new PatternId(designerKey, reader.GetStringUnsafe("reference"));
                var version = reader.GetStringUnsafe("version");
                var pattern = new Pattern(patternId, version) {
                    CategoryKey = reader.GetStringUnsafe("category"),
                    CreationTime = reader.GetDateTimeUnsafe("creation_time"),
                    HasPreview = reader.GetBooleanUnsafe("preview"),
                    LastModifiedTime = reader.GetDateTimeUnsafe("last_modified_time"),
                    Preferred = true,
                    Published = true,
                    Showcased = true,
                };

                result.Add(pattern);
            }

            return result;
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            // Intentionally left blank.
        }
    }
}
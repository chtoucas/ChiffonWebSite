namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    using Chiffon.Entities;
    using Chiffon.Internal;
    using Narvalo;
    using Narvalo.Data;

    public sealed class ListPatternsQuery : StoredProcedure<IEnumerable<Pattern>>
    {
        private bool? _published = true;

        public ListPatternsQuery(string connectionString, DesignerKey designerKey)
            : base(connectionString, "usp_ListPatterns")
        {
            Contract.Requires(connectionString != null);
            Contract.Requires(connectionString.Length != 0);

            DesignerKey = designerKey;
        }

        public string CategoryKey { get; set; }

        public DesignerKey DesignerKey { get; private set; }

        public bool? Published { get { return _published; } set { _published = value; } }

        protected override IEnumerable<Pattern> Execute(SqlDataReader reader)
        {
            CheckFor.StoredProcedure.Execute(reader);

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
            Check.NotNull(parameters, "The base class guarantees that the parameter is not null.");

            parameters.AddParameterUnsafe("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            parameters.AddParameterOrNullUnsafe("@published", SqlDbType.Bit, Published);
            parameters.AddParameterOrNullUnsafe("@category", SqlDbType.NVarChar, CategoryKey, !String.IsNullOrEmpty(CategoryKey));
        }
    }
}
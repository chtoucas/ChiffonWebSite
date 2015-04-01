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

    public sealed class ListCategoriesQuery : StoredProcedure<IEnumerable<Category>>
    {
        public ListCategoriesQuery(string connectionString, DesignerKey designerKey)
            : base(connectionString, "usp_ListCategories")
        {
            Contract.Requires(!String.IsNullOrEmpty(connectionString));

            DesignerKey = designerKey;
        }

        public DesignerKey DesignerKey { get; private set; }

        protected override IEnumerable<Category> Execute(SqlDataReader reader)
        {
            CheckFor.StoredProcedure.Execute(reader);

            var categories = new List<Category>();

            // Catégories du designer (avec au moins un motif).
            while (reader.Read())
            {
                var category = new Category(DesignerKey, reader.GetStringUnsafe("category")) {
                    DisplayName = reader.GetStringUnsafe("display_name"),
                    PatternsCount = reader.GetInt32Unsafe("patterns_count"),
                };
                categories.Add(category);
            }

            return categories;
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Check.NotNull(parameters, "The base class guarantees that the parameter is not null.");

            parameters.AddParameterUnsafe("@designer", SqlDbType.NVarChar, DesignerKey.Value);
        }
    }
}
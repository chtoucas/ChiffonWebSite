namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Chiffon.Entities;
    using Chiffon.Internal;
    using Narvalo;
    using Narvalo.Data;

    public sealed class ListDesignersQuery : StoredProcedure<IEnumerable<Designer>>
    {
        private readonly CultureInfo _culture;

        public ListDesignersQuery(string connectionString, CultureInfo culture)
            : base(connectionString, "usp_ListDesigners")
        {
            Require.NotNull(culture, "culture");
            Contract.Requires(connectionString != null);
            Contract.Requires(connectionString.Length != 0);

            _culture = culture;
        }

        protected override IEnumerable<Designer> Execute(SqlDataReader reader)
        {
            CheckFor.StoredProcedure.Execute(reader);

            var designers = new List<Designer>();

            while (reader.Read())
            {
                var designerKey = DesignerKey.Parse(reader.GetStringUnsafe("id"));

                var designer = new Designer(designerKey) {
                    AvatarCategory = reader.GetStringUnsafe("avatar_category"),
                    AvatarReference = reader.GetStringUnsafe("avatar_reference"),
                    AvatarVersion = reader.GetStringUnsafe("avatar_version"),
                    Email = reader.GetStringUnsafe("email_address"),
                    FirstName = reader.GetStringUnsafe("firstname"),
                    LastName = reader.GetStringUnsafe("lastname"),
                    Nickname = reader.MayGetStringUnsafe("nickname"),
                    Presentation = reader.GetStringUnsafe("presentation"),
                    WebSiteUrl = reader.MayGetStringUnsafe("website").Select(_ => new Uri(_)),
                };

                designers.Add(designer);
            }

            return designers;
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Check.NotNull(parameters, "The base class guarantees that the parameter is not null.");

            parameters.AddParameterUnsafe("@language", SqlDbType.Char, _culture.TwoLetterISOLanguageName);
        }
    }
}
namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public class ListDesignersQuery : StoredProcedure<IEnumerable<Designer>>
    {
        readonly CultureInfo _culture;

        public ListDesignersQuery(string connectionString, CultureInfo culture)
            : base(connectionString, "usp_ListDesigners")
        {
            Require.NotNull(culture, "culture");

            _culture = culture;
        }

        protected override IEnumerable<Designer> Execute(SqlDataReader reader)
        {
            Require.NotNull(reader, "reader");

            var designers = new List<Designer>();

            while (reader.Read()) {
                var designerKey = DesignerKey.Parse(reader.GetString("id"));

                var designer = new Designer(designerKey) {
                    AvatarCategory = reader.GetString("avatar_category"),
                    AvatarReference = reader.GetString("avatar_reference"),
                    AvatarVersion = reader.GetString("avatar_version"),
                    Email = reader.GetString("email_address"),
                    FirstName = reader.GetString("firstname"),
                    LastName = reader.GetString("lastname"),
                    Nickname = reader.MayGetString("nickname"),
                    Presentation = reader.GetString("presentation"),
                    WebsiteUrl = reader.MayGetString("website").Select(_ => new Uri(_)),
                };

                designers.Add(designer);
            }

            return designers;
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            parameters.AddParameter("@language", SqlDbType.Char, _culture.TwoLetterISOLanguageName);
        }
    }
}
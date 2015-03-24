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

        protected override IEnumerable<Designer> Execute(SqlDataReader rdr)
        {
            Require.NotNull(rdr, "rdr");

            var designers = new List<Designer>();

            while (rdr.Read()) {
                var designerKey = DesignerKey.Parse(rdr.GetString("id"));

                var designer = new Designer(designerKey) {
                    AvatarCategory = rdr.GetString("avatar_category"),
                    AvatarReference = rdr.GetString("avatar_reference"),
                    AvatarVersion = rdr.GetString("avatar_version"),
                    Email = rdr.GetString("email_address"),
                    FirstName = rdr.GetString("firstname"),
                    LastName = rdr.GetString("lastname"),
                    Nickname = rdr.MayGetString("nickname"),
                    Presentation = rdr.GetString("presentation"),
                    WebsiteUrl = rdr.MayGetString("website").Select(_ => new Uri(_)),
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
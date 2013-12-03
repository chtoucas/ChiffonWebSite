namespace Chiffon.Data.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Net.Mail;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Data;

    public class ListDesignersQuery : StoredProcedure<IEnumerable<Designer>>
    {
        public ListDesignersQuery(string connectionString, ChiffonCulture culture)
            : base(connectionString, "usp_ListDesigners")
        {
            Culture = culture;
        }

        public ChiffonCulture Culture { get; private set; }

        protected override IEnumerable<Designer> Execute(SqlDataReader rdr)
        {
            Requires.NotNull(rdr, "rdr");

            var designers = new List<Designer>();

            while (rdr.Read()) {
                var designerKey = DesignerKey.Parse(rdr.GetString("id"));

                var designer = new Designer(designerKey) {
                    AvatarCategory = rdr.GetString("avatar_category"),
                    AvatarReference = rdr.GetString("avatar_reference"),
                    AvatarVersion = rdr.GetString("avatar_version"),
                    EmailAddress = new MailAddress(rdr.GetString("email_address")),
                    FirstName = rdr.GetString("firstname"),
                    LastName = rdr.GetString("lastname"),
                    Nickname = rdr.MayGetString("nickname"),
                    Presentation = rdr.GetString("presentation"),
                    WebsiteUrl = rdr.MayGetString("website").Map(_ => new Uri(_)),
                };

                designers.Add(designer);
            }

            return designers;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@language", SqlDbType.Char, Culture.LanguageName);
        }
    }
}
namespace Chiffon.Data.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Net.Mail;
    using Chiffon.Entities;
    using Narvalo.Data;

    public class ListDesignersQuery : StoredProcedure<IEnumerable<Designer>>
    {
        public ListDesignersQuery(string connectionString, string languageName)
            : base(connectionString, "usp_ListDesigners")
        {
            LanguageName = languageName;
        }

        public string LanguageName { get; private set; }

        protected override IEnumerable<Designer> Execute(SqlDataReader rdr)
        {
            var designers = new List<Designer>();

            while (rdr.Read()) {
                var designerKey = DesignerKey.Parse(rdr.GetString("designer"));
                var designer = new Designer(designerKey) {
                    AvatarCategory = rdr.GetString("avatar_category"),
                    AvatarReference = rdr.GetString("avatar_reference"),
                    DisplayName = rdr.GetString("display_name"),
                    EmailAddress = new MailAddress(rdr.GetString("email_address")),
                    Presentation = rdr.GetString("presentation"),
                    WebSiteUrl = rdr.MayGetString("website_url").Map(_ => new Uri(_)),
                };

                designers.Add(designer);
            }

            return designers;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@language", SqlDbType.Char, LanguageName);
        }
    }
}
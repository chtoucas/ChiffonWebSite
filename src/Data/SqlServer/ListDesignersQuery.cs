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

        public override IEnumerable<Designer> Execute()
        {
            var designers = new List<Designer>();

            using (var cnx = new SqlConnection(ConnectionString)) {
                using (var cmd = CreateCommand(cnx)) {
                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (rdr.Read()) {
                            var designerKey = DesignerKey.Parse(rdr.GetString("designer"));
                            var designer = new Designer(designerKey) {
                                DisplayName = rdr.GetString("display_name"),
                                EmailAddress = new MailAddress(rdr.GetString("email_address")),
                                Presentation = rdr.GetString("presentation"),
                                Url = new Uri(rdr.GetString("uri")),
                            };

                            designers.Add(designer);
                        }
                    }
                }
            }

            return designers;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@language", SqlDbType.Char, LanguageName);
        }
    }
}
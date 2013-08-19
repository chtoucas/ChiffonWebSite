namespace Chiffon.Data.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Net.Mail;
    using Chiffon.Entities;
    using Narvalo.Data;

    public class GetDesignerQuery : StoredProcedure<Designer>
    {
        public GetDesignerQuery(string connectionString, DesignerKey designerKey, string languageName)
            : base(connectionString, "usp_GetDesigner")
        {
            DesignerKey = designerKey;
            LanguageName = languageName;

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        public DesignerKey DesignerKey { get; private set; }
        public string LanguageName { get; private set; }

        protected override Designer Execute(SqlDataReader rdr)
        {
            if (!rdr.Read()) { return null; }

            var designerKey = DesignerKey.Parse(rdr.GetString("designer"));
            
            return new Designer(designerKey) {
                AvatarCategory = rdr.GetString("avatar_category"),
                AvatarReference = rdr.GetString("avatar_reference"),
                EmailAddress = new MailAddress(rdr.GetString("email_address")),
                Firstname = rdr.GetString("firstname"),
                Lastname = rdr.GetString("lastname"),
                Presentation = rdr.GetString("presentation"),
                WebSiteUrl = rdr.MayGetString("website_url").Map(_ => new Uri(_)),
            };
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@language", SqlDbType.Char, LanguageName);
        }
    }
}
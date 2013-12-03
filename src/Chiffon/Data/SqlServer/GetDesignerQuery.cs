namespace Chiffon.Data.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Net.Mail;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Data;

    public class GetDesignerQuery : StoredProcedure<Designer>
    {
        public GetDesignerQuery(string connectionString, DesignerKey designerKey, ChiffonCulture culture)
            : base(connectionString, "usp_GetDesigner")
        {
            DesignerKey = designerKey;
            Culture = culture;

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        public ChiffonCulture Culture { get; private set; }
        public DesignerKey DesignerKey { get; private set; }

        protected override Designer Execute(SqlDataReader rdr)
        {
            Requires.NotNull(rdr, "rdr");

            if (!rdr.Read()) { return null; }

            return new Designer(DesignerKey) {
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
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@language", SqlDbType.Char, Culture.LanguageName);
        }
    }
}
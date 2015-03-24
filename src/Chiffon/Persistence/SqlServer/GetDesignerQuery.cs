namespace Chiffon.Persistence.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Data;

    public class GetDesignerQuery : StoredProcedure<Designer>
    {
        readonly CultureInfo _culture;
        readonly DesignerKey _designerKey;

        public GetDesignerQuery(string connectionString, DesignerKey designerKey, CultureInfo culture)
            : base(connectionString, "usp_GetDesigner")
        {
            Require.NotNull(culture, "culture");

            _designerKey = designerKey;
            _culture = culture;

            CommandBehavior = CommandBehavior.CloseConnection | CommandBehavior.SingleRow;
        }

        protected override Designer Execute(SqlDataReader rdr)
        {
            Require.NotNull(rdr, "rdr");

            if (!rdr.Read()) { return null; }

            return new Designer(_designerKey) {
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
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            parameters.AddParameter("@designer", SqlDbType.NVarChar, _designerKey.Value);
            parameters.AddParameter("@language", SqlDbType.Char, _culture.TwoLetterISOLanguageName);
        }
    }
}
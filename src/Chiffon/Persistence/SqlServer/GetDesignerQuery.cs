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

        protected override Designer Execute(SqlDataReader reader)
        {
            Require.NotNull(reader, "reader");

            if (!reader.Read()) { return null; }

            return new Designer(_designerKey) {
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
        }

        protected override void PrepareParameters(SqlParameterCollection parameters)
        {
            Require.NotNull(parameters, "parameters");

            parameters.AddParameterUnsafe("@designer", SqlDbType.NVarChar, _designerKey.Value);
            parameters.AddParameterUnsafe("@language", SqlDbType.Char, _culture.TwoLetterISOLanguageName);
        }
    }
}
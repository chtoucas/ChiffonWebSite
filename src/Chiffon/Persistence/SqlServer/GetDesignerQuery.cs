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
                AvatarCategory = reader.GetStringUnsafe("avatar_category"),
                AvatarReference = reader.GetStringUnsafe("avatar_reference"),
                AvatarVersion = reader.GetStringUnsafe("avatar_version"),
                Email = reader.GetStringUnsafe("email_address"),
                FirstName = reader.GetStringUnsafe("firstname"),
                LastName = reader.GetStringUnsafe("lastname"),
                Nickname = reader.MayGetStringUnsafe("nickname"),
                Presentation = reader.GetStringUnsafe("presentation"),
                WebsiteUrl = reader.MayGetStringUnsafe("website").Select(_ => new Uri(_)),
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
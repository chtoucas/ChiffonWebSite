namespace Chiffon.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;

    public class GetDesignerVMQuery : StoredProcedure<DesignerViewModel>
    {
        public GetDesignerVMQuery(string connectionString, DesignerKey designerKey, string languageName)
            : base(connectionString, "usp_GetDesignerVm")
        {
            DesignerKey = designerKey;
            LanguageName = languageName;

            CommandBehavior = CommandBehavior.CloseConnection;
        }

        public DesignerKey DesignerKey { get; private set; }
        public string LanguageName { get; private set; }

        protected override DesignerViewModel Execute(SqlDataReader rdr)
        {
            var result = new DesignerViewModel();

            // Informations sur le designer.
            if (!rdr.Read()) { return null; }

            result.Designer = new DesignerViewItem {
                DisplayName = rdr.GetString("display_name"),
                EmailAddress = rdr.GetString("email_address"),
                Key = DesignerKey,
                Presentation = rdr.GetString("presentation")
            };

            rdr.NextResult();

            // Catégories du designer (avec au moins un motif).
            var categories = new List<CategoryViewItem>();
            while (rdr.Read()) {
                var category = new CategoryViewItem {
                    DisplayName = rdr.GetString("display_name"),
                    PatternCount = rdr.GetInt32("pattern_count"),
                    Key = rdr.GetString("category"),
                };
                categories.Add(category);
            }
            result.Categories = categories;

            return result;
        }

        protected override void PrepareCommand(SqlCommand command)
        {
            command.AddParameter("@designer", SqlDbType.NVarChar, DesignerKey.Value);
            command.AddParameter("@language", SqlDbType.Char, LanguageName);
        }
    }
}
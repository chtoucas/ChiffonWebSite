namespace Chiffon.Data.SqlServer.ViewModelQueries
{
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;

    internal static class SqlDataReaderExtensions
    {
        public static CategoryViewItem GetCategoryItem(this SqlDataReader rdr)
        {
            return new CategoryViewItem {
                DisplayName = rdr.GetString("display_name"),
                PatternCount = rdr.GetInt32("pattern_count"),
                Key = rdr.GetString("category"),
            };
        }

        public static DesignerViewItem GetDesignerItem(this SqlDataReader rdr, DesignerKey designerKey)
        {
            return new DesignerViewItem {
                DisplayName = rdr.GetString("display_name"),
                EmailAddress = rdr.GetString("email_address"),
                Key = designerKey,
                Presentation = rdr.GetString("presentation")
            };
        }

        public static PatternViewItem GetPatternItem(this SqlDataReader rdr)
        {
            return new PatternViewItem {
                CategoryKey = rdr.GetString("category"),
                DesignerKey = DesignerKey.Parse(rdr.GetString("designer")),
                DesignerName = rdr.GetString("designer_name"),
                Reference = rdr.GetString("reference"),
            };
        }

        public static PatternViewItem GetPatternItem(this SqlDataReader rdr, DesignerKey designerKey, string designerName)
        {
            return new PatternViewItem {
                CategoryKey = rdr.GetString("category"),
                DesignerKey = designerKey,
                DesignerName = designerName,
                Reference = rdr.GetString("reference"),
            };
        }

        public static PatternViewItem GetPatternItem(this SqlDataReader rdr, DesignerKey designerKey, string categoryKey, string designerName)
        {
            return new PatternViewItem {
                CategoryKey = categoryKey,
                DesignerKey = designerKey,
                DesignerName = designerName,
                Reference = rdr.GetString("reference"),
            };
        }
    }
}
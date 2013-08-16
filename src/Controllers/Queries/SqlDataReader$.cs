namespace Chiffon.Controllers.Queries
{
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;

    internal static class SqlDataReaderExtensions
    {
        public static CategoryItem GetCategory(this SqlDataReader rdr)
        {
            return new CategoryItem {
                DisplayName = rdr.GetStringColumn("display_name"),
                PatternCount = rdr.GetInt32Column("pattern_count"),
                Key = rdr.GetStringColumn("category"),
            };
        }

        public static DesignerItem GetDesigner(this SqlDataReader rdr, DesignerKey designerKey)
        {
            return new DesignerItem {
                DisplayName = rdr.GetStringColumn("display_name"),
                EmailAddress = rdr.GetStringColumn("email_address"),
                Key = designerKey,
                Presentation = rdr.GetStringColumn("presentation")
            };
        }

        public static PatternItem GetPattern(this SqlDataReader rdr)
        {
            return new PatternItem {
                CategoryKey = rdr.GetStringColumn("category"),
                DesignerKey = DesignerKey.Parse(rdr.GetStringColumn("designer")),
                DesignerName = rdr.GetStringColumn("designer_name"),
                Reference = rdr.GetStringColumn("reference"),
            };
        }

        public static PatternItem GetPattern(this SqlDataReader rdr, DesignerKey designerKey, string designerName)
        {
            return new PatternItem {
                CategoryKey = rdr.GetStringColumn("category"),
                DesignerKey = designerKey,
                DesignerName = designerName,
                Reference = rdr.GetStringColumn("reference"),
            };
        }

        public static PatternItem GetPattern(this SqlDataReader rdr, DesignerKey designerKey, string categoryKey, string designerName)
        {
            return new PatternItem {
                CategoryKey = categoryKey,
                DesignerKey = designerKey,
                DesignerName = designerName,
                Reference = rdr.GetStringColumn("reference"),
            };
        }
    }
}
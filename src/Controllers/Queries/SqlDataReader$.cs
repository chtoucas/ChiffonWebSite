namespace Chiffon.Controllers.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.ViewModels;
    using Narvalo.Data;

    internal static class SqlDataReaderExtensions
    {
        public static List<CategoryItem> GetCategories(this SqlDataReader rdr)
        {
            var categories = new List<CategoryItem>();

            while (rdr.Read()) {
                var category = new CategoryItem {
                    DisplayName = rdr.GetStringColumn("display_name"),
                    PatternCount = rdr.GetInt32Column("pattern_count"),
                    Reference = rdr.GetStringColumn("reference"),
                };
                categories.Add(category);
            }

            return categories;
        }

        public static DesignerItem GetDesigner(this SqlDataReader rdr, DesignerKey designer)
        {
            if (rdr.Read()) {
                return new DesignerItem {
                    Key = designer,
                    DisplayName = rdr.GetStringColumn("display_name"),
                    EmailAddress = rdr.GetStringColumn("email_address"),
                    Presentation = rdr.GetStringColumn("presentation")
                };
            }
            else {
                throw new InvalidOperationException("XXX");
            }
        }

        public static PatternItem GetPattern(this SqlDataReader rdr, DesignerKey designer, string displayName)
        {
            return new PatternItem {
                DesignerKey = designer,
                DesignerName = displayName,
                Reference = rdr.GetStringColumn("reference"),
            };
        }

        public static List<PatternItem> GetPatterns(this SqlDataReader rdr, DesignerKey designer, string displayName)
        {
            var patterns = new List<PatternItem>();

            while (rdr.Read()) {
                patterns.Add(rdr.GetPattern(designer, displayName));
            }

            return patterns;
        }
    }
}
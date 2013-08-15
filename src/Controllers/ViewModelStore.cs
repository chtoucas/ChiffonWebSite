namespace Chiffon.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Data;

    public class ViewModelStore
    {
        readonly DbHelper _dbHelper;

        public ViewModelStore(DbHelper dbHelper)
        {
            Requires.NotNull(dbHelper, "dbHelper");

            _dbHelper = dbHelper;
        }

        public List<PatternItem> Home()
        {
            var model = new List<PatternItem>();

            using (var cnx = _dbHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_getShowcasedPatterns";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (rdr.Read()) {
                            var preview = new PatternItem {
                                DesignerKey = DesignerKey.Parse(rdr.GetStringColumn("designer_id")).Value,
                                DesignerName = rdr.GetStringColumn("designer_name"),
                                Reference = rdr.GetStringColumn("reference"),
                            };
                            model.Add(preview);
                        }
                    }
                }
            }

            return model;
        }

        public DesignerViewModel Designer(DesignerKey designer, string language)
        {
            var model = new DesignerViewModel();

            using (var cnx = _dbHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_fo_getDesigner";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designer.Key;
                    p.Add("@language", SqlDbType.Char).Value = language;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        // Informations sur le designer.
                        model.Designer = ReadDesigner_(rdr, designer);

                        // Catégories du designer (avec au moins un motif).
                        rdr.NextResult();
                        model.Categories = ReadCategories_(rdr);

                        // Les motifs du designer.
                        rdr.NextResult();
                        model.Previews = ReadPatterns_(rdr, designer, model.Designer.DisplayName);
                    }
                }
            }
            return model;
        }

        public DesignerViewModel Category(DesignerKey designer, string categoryKey, string language)
        {
            var model = new CategoryViewModel();

            using (var cnx = _dbHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_fo_getDesignerCategory";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designer.Key;
                    p.Add("@category", SqlDbType.NVarChar).Value = categoryKey;
                    p.Add("@language", SqlDbType.Char).Value = language;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        // Informations sur le designer.
                        model.Designer = ReadDesigner_(rdr, designer);

                        // Catégories du designer (avec au moins un motif).
                        rdr.NextResult();
                        model.Categories = ReadCategories_(rdr);

                        // Les motifs de la catégorie.
                        rdr.NextResult();
                        model.Previews = ReadPatterns_(rdr, designer, model.Designer.DisplayName);
                    }
                }
            }

            return model;
        }

        public PatternViewModel Pattern(DesignerKey designer, string reference, string language)
        {
            var model = new PatternViewModel();

            using (var cnx = _dbHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_fo_getPattern";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@reference", SqlDbType.NVarChar).Value = reference;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designer.Key;
                    p.Add("@language", SqlDbType.Char).Value = language;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        // Informations sur le designer.
                        model.Designer = ReadDesigner_(rdr, designer);

                        // Catégories du designer (avec au moins un motif).
                        rdr.NextResult();
                        model.Categories = ReadCategories_(rdr);

                        // Détails sur le motif.
                        rdr.NextResult();
                        if (rdr.Read()) {
                            model.Pattern = ReadPattern_(rdr, designer, model.Designer.DisplayName);
                        }

                        // Autres motifs dans la même catégorie.
                        rdr.NextResult();
                        model.Previews = ReadPatterns_(rdr, designer, model.Designer.DisplayName);
                    }
                }
            }

            return model;
        }


        static List<CategoryItem> ReadCategories_(SqlDataReader rdr)
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

        static DesignerItem ReadDesigner_(SqlDataReader rdr, DesignerKey designer)
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

        static PatternItem ReadPattern_(SqlDataReader rdr, DesignerKey designer, string displayName)
        {
            return new PatternItem {
                DesignerKey = designer,
                DesignerName = displayName,
                Reference = rdr.GetStringColumn("reference"),
            };
        }

        static List<PatternItem> ReadPatterns_(SqlDataReader rdr, DesignerKey designer, string displayName)
        {
            var patterns = new List<PatternItem>();

            while (rdr.Read()) {
                patterns.Add(ReadPattern_(rdr, designer, displayName));
            }

            return patterns;
        }
    }
}
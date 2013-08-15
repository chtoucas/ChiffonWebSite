namespace Chiffon.Controllers
{
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
            var model = new DesignerViewModel {
                DesignerKey = designer,
            };

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

                        if (rdr.Read()) {
                            model.DisplayName = rdr.GetStringColumn("display_name");
                            model.EmailAddress = rdr.GetStringColumn("email_address");
                            model.Presentation = rdr.GetStringColumn("presentation");
                        }

                        // Catégories du designer (avec au moins un motif).

                        rdr.NextResult();

                        var categories = new List<CategoryItem>();

                        while (rdr.Read()) {
                            var category = new CategoryItem {
                                DisplayName = rdr.GetStringColumn("display_name"),
                                PatternCount = rdr.GetInt32Column("pattern_count"),
                                Reference = rdr.GetStringColumn("reference"),
                            };
                            categories.Add(category);
                        }

                        model.Categories = categories;

                        // Les motifs du designer.

                        rdr.NextResult();

                        var previews = new List<PatternItem>();

                        while (rdr.Read()) {
                            var preview = new PatternItem {
                                DesignerKey = designer,
                                DesignerName = model.DisplayName,
                                Reference = rdr.GetStringColumn("reference"),
                            };
                            previews.Add(preview);
                        }

                        model.Previews = previews;
                    }
                }
            }
            return model;
        }

        public DesignerViewModel Category(DesignerKey designer, string categoryKey, string language)
        {
            var model = new CategoryViewModel {
                DesignerKey = designer,
            };

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

                        if (rdr.Read()) {
                            model.DisplayName = rdr.GetStringColumn("display_name");
                            model.EmailAddress = rdr.GetStringColumn("email_address");
                            model.Presentation = rdr.GetStringColumn("presentation");
                        }

                        // Catégories du designer (avec au moins un motif).

                        rdr.NextResult();

                        var categories = new List<CategoryItem>();

                        while (rdr.Read()) {
                            var category = new CategoryItem {
                                DisplayName = rdr.GetStringColumn("display_name"),
                                PatternCount = rdr.GetInt32Column("pattern_count"),
                                Reference = rdr.GetStringColumn("reference"),
                            };
                            categories.Add(category);
                        }

                        model.Categories = categories;

                        // Les motifs de la catégorie.

                        rdr.NextResult();

                        var previews = new List<PatternItem>();

                        while (rdr.Read()) {
                            var preview = new PatternItem {
                                DesignerKey = designer,
                                DesignerName = model.DisplayName,
                                Reference = rdr.GetStringColumn("reference"),
                            };
                            previews.Add(preview);
                        }

                        model.Previews = previews;
                    }
                }
            }
            return model;
        }

        public PatternViewModel Pattern(DesignerKey designer, string reference, string language)
        {
            var model = new PatternViewModel {
                DesignerKey = designer,
            };

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

                        if (rdr.Read()) {
                            model.DisplayName = rdr.GetStringColumn("display_name");
                            model.EmailAddress = rdr.GetStringColumn("email_address");
                            model.Presentation = rdr.GetStringColumn("presentation");
                        }

                        // Catégories du designer (avec au moins un motif).

                        rdr.NextResult();

                        var categories = new List<CategoryItem>();

                        while (rdr.Read()) {
                            var category = new CategoryItem {
                                DisplayName = rdr.GetStringColumn("display_name"),
                                PatternCount = rdr.GetInt32Column("pattern_count"),
                                Reference = rdr.GetStringColumn("reference"),
                            };
                            categories.Add(category);
                        }

                        model.Categories = categories;

                        // Détails sur le motif.

                        rdr.NextResult();

                        if (rdr.Read()) {
                            model.Pattern = new PatternItem {
                                DesignerKey = designer,
                                DesignerName = model.DisplayName,
                                Reference = rdr.GetStringColumn("reference"),
                            };
                        }
                        
                        // Autres motifs dans la même catégorie.

                        rdr.NextResult();

                        var previews = new List<PatternItem>();

                        while (rdr.Read()) {
                            var preview = new PatternItem {
                                DesignerKey = designer,
                                DesignerName = model.DisplayName,
                                Reference = rdr.GetStringColumn("reference"),
                            };
                            previews.Add(preview);
                        }

                        model.Previews = previews;
                    }
                }
            }

            return model;
        }
    }
}
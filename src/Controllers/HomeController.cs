namespace Chiffon.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Data;

    [SeoPolicy(RobotsDirective = "index, follow")]
    public partial class HomeController : Controller
    {
        DbHelper _dbHelper;

        public HomeController(DbHelper dbHelper)
        {
            Requires.NotNull(dbHelper, "dbHelper");

            _dbHelper = dbHelper;
        }

        [HttpGet]
        [Html("home")]
        public virtual ActionResult Index()
        {
            var model = new List<PatternPreviewViewModel>();

            using (var cnx = _dbHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_getShowcasedPatterns";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (rdr.Read()) {
                            var preview = new PatternPreviewViewModel {
                                DesignerKey = DesignerKey.Parse(rdr.GetStringColumn("designer_id")).Value,
                                DesignerName = rdr.GetStringColumn("designer_name"),
                                Reference = rdr.GetStringColumn("reference"),
                            };
                            model.Add(preview);
                        }
                    }
                }
            }

            // Cf. http://stackoverflow.com/questions/3797182/how-to-correctly-canonicalize-a-url-in-an-asp-net-mvc-application
            // & https://github.com/schourode/canonicalize
            ViewBag.CanonicalLink = Url.Action("Index", MVC.Home.Name, null /* routeValues */, "http", null);

            return View(ViewPath.Index, model);
        }

        [HttpGet]
        [Html("about")]
        public virtual ActionResult About()
        {
            return View(ViewPath.About);
        }

        [HttpGet]
        [Html("contact")]
        public virtual ActionResult Contact()
        {
            return View(ViewPath.Contact);
        }

        [HttpGet]
        [Html("newsletter")]
        public virtual ActionResult Newsletter()
        {
            return View(ViewPath.Newsletter);
        }

        static class ViewPath
        {
            public const string Index = "~/Views/Home/Index.cshtml";
            public const string About = "~/Views/Home/About.cshtml";
            public const string Contact = "~/Views/Home/Contact.cshtml";
            public const string Newsletter = "~/Views/Home/Newsletter.cshtml";
        }
    }
}

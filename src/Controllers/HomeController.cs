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

    public partial class HomeController : BaseController
    {
        DbHelper _dbHelper;

        public HomeController(DbHelper dbHelper)
        {
            Requires.NotNull(dbHelper, "dbHelper");

            _dbHelper = dbHelper;
        }

        [HttpGet]
        [BodyId("home")]
        [MetaRobots(IndexAndFollow)]
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
            // Cf. http://blog.muonlab.com/2011/11/27/simplistic-canonical-linking-in-asp-net-mvc/
            ViewBag.CanonicalLink = Url.Action("Index", MVC.Home.Name, null /* routeValues */, "http", null);

            return View(ViewPath.Home.Index, model);
        }

        [HttpGet]
        [BodyId("about")]
        [MetaRobots(IndexAndFollow)]
        public virtual ActionResult About()
        {
            return View(ViewPath.Home.About);
        }

        [HttpGet]
        [BodyId("contact")]
        [MetaRobots(IndexAndFollow)]
        public virtual ActionResult Contact()
        {
            return View(ViewPath.Home.Contact);
        }

        [HttpGet]
        [BodyId("newsletter")]
        [MetaRobots(IndexAndFollow)]
        public virtual ActionResult Newsletter()
        {
            return View(ViewPath.Home.Newsletter);
        }
    }
}

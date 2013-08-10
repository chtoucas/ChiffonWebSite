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

            MarkForIndexation();

            return View(ViewName.Home.Index, model);
        }

        [HttpGet]
        [BodyId("about")]
        public virtual ActionResult About()
        {
            MarkForIndexation();

            return View(ViewName.Home.About);
        }

        [HttpGet]
        [BodyId("contact")]
        public virtual ActionResult Contact()
        {
            MarkForIndexation();

            return View(ViewName.Home.Contact);
        }

        [HttpGet]
        [BodyId("newsletter")]
        public virtual ActionResult Newsletter()
        {
            MarkForIndexation();

            return View(ViewName.Home.Newsletter);
        }
    }
}

namespace Chiffon.WebSite.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public ActionResult About()
        {
            return View("~/Views/Home/About.cshtml");
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View("~/Views/Home/Contact.cshtml");
        }
    }
}

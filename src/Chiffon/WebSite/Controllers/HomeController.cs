namespace Chiffon.WebSite.Controllers
{
    using System.Web.Mvc;

    public partial class HomeController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public virtual ActionResult About()
        {
            return View("~/Views/Home/About.cshtml");
        }

        [HttpGet]
        public virtual ActionResult Contact()
        {
            return View("~/Views/Home/Contact.cshtml");
        }
    }
}

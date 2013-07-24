namespace Chiffon.WebSite.Controllers
{
    using System.Web.Mvc;

    public class MembersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}

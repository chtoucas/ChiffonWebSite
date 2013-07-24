namespace Chiffon.WebSite.Controllers
{
    using System.Web.Mvc;

    public class MemberController : Controller
    {
        [HttpGet]
        public ActionResult Index(string key)
        {
            return View("~/Views/Member/Index.cshtml");
        }
    }
}

namespace Chiffon.Controllers
{
    using System.Web.Mvc;

    public class MemberController : Controller
    {
        [HttpGet]
        public ActionResult Index(string designer)
        {
            return View("~/Views/Member/Index.cshtml");
        }
    }
}

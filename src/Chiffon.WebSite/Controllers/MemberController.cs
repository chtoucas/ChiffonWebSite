namespace Chiffon.WebSite.Controllers
{
    using System.Web.Mvc;

    public partial class MemberController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index(string key)
        {
            return View("~/Views/Member/Index.cshtml");
        }
    }
}

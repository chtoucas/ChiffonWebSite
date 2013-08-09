namespace Chiffon.Controllers
{
    using System.Web.Mvc;

    public partial class MemberController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index(string designer)
        {
            return View("~/Views/Member/Index.cshtml");
        }

        [HttpGet]
        public virtual ActionResult Pattern(string designer, string reference)
        {
            return View("~/Views/Member/Pattern.cshtml");
        }
    }
}

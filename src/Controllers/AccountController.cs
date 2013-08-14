namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;

    public partial class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            return View(ViewName.Account.Register);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        [HttpGet]
        public ActionResult ModalRegister(string returnUrl)
        {
            return PartialView(ViewName.Account.Register);
        }
    }
}

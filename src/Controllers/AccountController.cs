namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;

    public class AccountController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View(ViewName.Account.Login);
        }

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

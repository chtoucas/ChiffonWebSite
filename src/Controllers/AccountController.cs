namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;

    public partial class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public virtual ActionResult Register(string returnUrl)
        {
            return View(ViewPath.Account.Register);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        [HttpGet]
        public virtual ActionResult ModalRegister(string returnUrl)
        {
            return PartialView(ViewPath.Account.Register);
        }
    }
}

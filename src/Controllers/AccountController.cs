namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;

    public partial class AccountController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public virtual ActionResult Register()
        {
            return View(ViewPath.Account.Register);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        [HttpGet]
        public virtual ActionResult ModalRegister()
        {
            return PartialView(ViewPath.Account.Register);
        }
    }
}

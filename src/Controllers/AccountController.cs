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
            return View(ViewName.Account.Register);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public virtual ActionResult ModalRegister()
        {
            return PartialView(ViewName.Account.Register);
        }
    }
}

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
            return View(ViewPath.Register);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        [HttpGet]
        public virtual ActionResult ModalRegister(string returnUrl)
        {
            return PartialView(ViewPath.Register);
        }

        // AccountController.
        static class ViewPath
        {
            public const string Register = "~/Views/Account/Register.cshtml";
        }
    }
}

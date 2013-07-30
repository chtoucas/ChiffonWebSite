namespace Chiffon.WebSite.Controllers
{
    using System.Web.Mvc;

    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        //[AllowAnonymous]
        //[ChildActionOnly]
        //public ActionResult ModalRegister()
        //{
        //    return PartialView("~/Views/Account/_Register.cshtml");
        //}
    }
}

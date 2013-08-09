﻿namespace Chiffon.Controllers
{
    using System.Web.Mvc;

    public partial class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public virtual ActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public virtual ActionResult ModalRegister()
        {
            return PartialView("~/Views/Account/Register.cshtml");
        }
    }
}

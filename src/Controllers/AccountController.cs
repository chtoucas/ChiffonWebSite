﻿namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;

    public class AccountController : PageController
    {
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            ViewBag.CanonicalLink = SiteMap.Login().ToString();

            return View(ViewName.Account.Login);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.CanonicalLink = SiteMap.Register().ToString();

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

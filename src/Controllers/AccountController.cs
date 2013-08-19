namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;

    public class AccountController : PageController
    {
        public AccountController(ChiffonEnvironment environment, ISiteMap siteMap) : base(environment, siteMap) { }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            ViewBag.Title = SR.Account_Login_Title;
            ViewBag.MetaDescription = SR.Account_Login_Description;
            ViewBag.CanonicalLink = SiteMap.Login().ToString();

            return View(ViewName.Account.Login);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.Title = SR.Account_Register_Title;
            ViewBag.MetaDescription = SR.Account_Register_Description;
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

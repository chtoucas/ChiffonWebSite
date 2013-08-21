namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;

    public class ContactController : PageController
    {
        public ContactController(ChiffonEnvironment environment, ISiteMap siteMap)
            : base(environment, siteMap) { }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            ViewBag.Title = SR.Contact_Login_Title;
            ViewBag.MetaDescription = SR.Contact_Login_Description;
            ViewBag.CanonicalLink = SiteMap.Login().ToString();

            return View(ViewName.Contact.Login);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.Title = SR.Contact_Register_Title;
            ViewBag.MetaDescription = SR.Contact_Register_Description;
            ViewBag.CanonicalLink = SiteMap.Register().ToString();

            return View(ViewName.Contact.Register);
        }

        //[AllowAnonymous]
        //[ChildActionOnly]
        //[HttpGet]
        //public ActionResult ModalRegister(string returnUrl)
        //{
        //    return PartialView(ViewName.Account.Register);
        //}

        [HttpGet]
        public ActionResult Newsletter()
        {
            ViewBag.Title = SR.Contact_Newsletter_Title;
            ViewBag.MetaDescription = SR.Contact_Newsletter_Description;
            ViewBag.CanonicalLink = SiteMap.Newsletter().ToString();

            return View(ViewName.Contact.Newsletter);
        }
    }
}

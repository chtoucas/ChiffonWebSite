namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;

    [AllowAnonymous]
    public class ContactController : PageController
    {
        public ContactController(ChiffonEnvironment environment, ISiteMap siteMap)
            : base(environment, siteMap) { }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            ViewBag.Title = SR.Contact_Login_Title;
            ViewBag.MetaDescription = SR.Contact_Login_Description;
            ViewBag.CanonicalLink = SiteMap.Login().ToString();

            if (Request.IsAjaxRequest()) {
                return PartialView(ViewName.Contact.Login);
            }
            else {
                return View(ViewName.Contact.Login);
            }
        }

        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.Title = SR.Contact_Register_Title;
            ViewBag.MetaDescription = SR.Contact_Register_Description;
            ViewBag.CanonicalLink = SiteMap.Register().ToString();

            if (Request.IsAjaxRequest()) {
                return PartialView(ViewName.Contact.Register);
            }
            else {
                return View(ViewName.Contact.Register);
            }
        }

        [HttpGet]
        public ActionResult Newsletter()
        {
            ViewBag.Title = SR.Contact_Newsletter_Title;
            ViewBag.MetaDescription = SR.Contact_Newsletter_Description;
            ViewBag.CanonicalLink = SiteMap.Newsletter().ToString();
            ViewBag.MainMenuClass = "newsletter";

            return View(ViewName.Contact.Newsletter);
        }
    }
}

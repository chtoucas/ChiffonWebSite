namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;

    public class MailMergeController : Controller
    {
        public ActionResult ForgottenPassword()
        {
            return View(Constants.ViewName.MailMerge.ForgottenPassword);
        }

        public ActionResult Welcome()
        {
            return View(Constants.ViewName.MailMerge.Welcome);
        }
    }
}
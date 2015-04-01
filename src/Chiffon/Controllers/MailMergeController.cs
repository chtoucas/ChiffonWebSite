namespace Chiffon.Controllers
{
    using System.Web.Mvc;

    public sealed class MailMergeController : Controller
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
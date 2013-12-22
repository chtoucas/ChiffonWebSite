namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;

    // Notes:
    // - utiliser ISO-8859-1
    // - utiliser Quoted-Printable
    // - vérifier SPF, DKIM
    // - mode authentifié uniquement
    // - beacon
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
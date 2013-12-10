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
    public class MailController : Controller
    {
        public ActionResult ForgottenPassword()
        {
            return View(ViewName.Mail.ForgottenPassword);
        }

        public ActionResult Welcome()
        {
            return View(ViewName.Mail.Welcome);
        }
    }
}
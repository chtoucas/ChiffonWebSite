namespace Chiffon.Controllers
{
    using System;
    using System.Net.Mail;
    using Mvc.Mailer;

    public interface IMailController
    {
        MvcMailMessage Welcome(MailAddress emailAddress, string publicKey, Uri baseUri, string languageName);
    }
}
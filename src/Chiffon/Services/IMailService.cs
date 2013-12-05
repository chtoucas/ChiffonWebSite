namespace Chiffon.Services
{
    using System;
    using System.Net.Mail;
    using Mvc.Mailer;

    public interface IMailService
    {
        MvcMailMessage Welcome(
             MailAddress emailAddress,
             string publicKey,
             Uri baseUri,
             string languageName);
    }
}
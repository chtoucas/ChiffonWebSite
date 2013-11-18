namespace Chiffon.Mailers
{
    using System;
    using System.Net.Mail;
    using Mvc.Mailer;

    [CLSCompliant(false)]
    public interface IAccountMailer
    {
        MvcMailMessage Welcome(MailAddress emailAddress, string publicKey, Uri baseUri, string languageName);
    }
}
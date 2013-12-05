namespace Chiffon.Common
{
    using System;
    using System.Net.Mail;
    using Chiffon.Resources;
    using Mvc.Mailer;
    using Narvalo;

    public class MailService : MvcMailer
    {
        public MailService()
            : base()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome(
            MailAddress emailAddress,
            string publicKey,
            Uri baseUri,
            string languageName)
        {
            Requires.NotNull(baseUri, "baseUri");

            ViewBag.LanguageName = languageName;
            ViewBag.EmailAddress = emailAddress.Address;
            ViewBag.Password = publicKey;
            ViewBag.SiteUrl = baseUri.ToString();

            return Populate(x => {
                x.ViewName = "Welcome";
                x.Subject = MailResources.Welcome_Subject;
                x.To.Add(emailAddress);
            });
        }
    }
}
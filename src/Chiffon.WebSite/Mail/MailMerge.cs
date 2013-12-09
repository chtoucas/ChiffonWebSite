namespace Chiffon.Mail
{
    using System;
    using System.Net.Mail;
    using Chiffon.Resources;
    using Narvalo;

    public class MailMerge : MailMessageController
    {
        public MailMerge()
            : base()
        {
            MasterName = "_Layout";
        }

        public MailMessage Welcome(
            MailAddress emailAddress,
            string password,
            Uri baseUri,
            string languageName)
        {
            Requires.NotNull(baseUri, "baseUri");

            ViewBag.EmailAddress = emailAddress.Address;
            ViewBag.LanguageName = languageName;
            ViewBag.Password = password;
            ViewBag.SiteUrl = baseUri.ToString();

            var message = MailMessageX("Welcome", "_Layout");

            message.Subject = MailResources.Welcome_Subject;
            message.To.Add(emailAddress);

            return message;
        }
    }
}
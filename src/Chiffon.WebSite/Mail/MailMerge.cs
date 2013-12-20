namespace Chiffon.Mail
{
    using System;
    using System.Globalization;
    using System.Net.Mail;
    using Chiffon.Common;
    using Chiffon.Resources;
    using Narvalo;

    public class MailMerge : MailController
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

        public MailMessage NewMember(
            MailAddress emailAddress,
            string firstName,
            string lastName,
            string companyName)
        {
            var email = emailAddress.Address;

            ViewBag.EmailAddress = email;
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.CompanyName = companyName;

            var message = MailMessageX("NewMember", "_Layout");

            message.Subject = String.Format(CultureInfo.InvariantCulture,
                "Nouvelle inscription sur le site : {0} {1}.", firstName, lastName);
            message.To.Add(Constants.ContactAddress);

            return message;
        }

        public MailMessage NewMessage(
            MailAddress emailAddress,
            string name,
            string bodyText)
        {
            var email = emailAddress.Address;

            ViewBag.EmailAddress = email;
            ViewBag.Name = name;
            ViewBag.BodyText = bodyText;

            var message = MailMessageX("NewMessage", "_Layout");

            message.Subject = String.Format(CultureInfo.InvariantCulture,
                "Nouveau message sur le site de {0}.", email);
            message.To.Add(Constants.ContactAddress);

            return message;
        }
    }
}
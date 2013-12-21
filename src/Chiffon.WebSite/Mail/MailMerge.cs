namespace Chiffon.Mail
{
    using System;
    using System.Globalization;
    using System.Net.Mail;
    using Chiffon.Common;
    using Chiffon.Resources;
    using Narvalo;

    // TODO: Utiliser SmartFormat.NET ou StringTemplate.
    public class MailMerge : MailController
    {
        public MailMerge()
            : base()
        {
            MasterName = "_Layout";
        }

        protected static string NewMemberAlertTpl
        {
            get { return Chiffon.Properties.Resources.NewMemberAlertEmail; }
        }

        protected static string NewMessageAlertTpl
        {
            get { return Chiffon.Properties.Resources.NewMessageAlertEmail; }
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

        public MailMessage NewMemberAlert(
            MailAddress emailAddress,
            string firstName,
            string lastName,
            string companyName)
        {
            var message = new MailMessage {
                Body = String.Format(CultureInfo.InvariantCulture, NewMemberAlertTpl, firstName, lastName, companyName, emailAddress.Address),
                IsBodyHtml = false,
                Subject = String.Format(CultureInfo.InvariantCulture,
                  "Nouvelle inscription sur le site : {0} {1}.", firstName, lastName)
            };
            message.To.Add(Constants.ContactAddress);

            return message;
        }

        public MailMessage NewMessageAlert(MailAddress emailAddress, string bodyText)
        {
            var email = emailAddress.Address;
            var displayName = emailAddress.DisplayName;

            var message = new MailMessage {
                Body = String.Format(CultureInfo.InvariantCulture, NewMessageAlertTpl, displayName, email, bodyText),
                IsBodyHtml = false,
                Subject = String.Format(CultureInfo.InvariantCulture,
                "Nouveau message sur le site de la part de {0}.", email)
            };
            message.To.Add(Constants.ContactAddress);

            return message;
        }
    }
}
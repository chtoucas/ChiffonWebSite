namespace Chiffon.Mail
{
    using System;
    using System.Globalization;
    using System.Net.Mail;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Infrastructure.Messaging;
    using Chiffon.Resources;
    using Narvalo;

    // TODO: Utiliser SmartFormat.NET ou StringTemplate.
    public class MailMerge : MailController, IMailMerge
    {
        readonly ISiteMap _siteMap;

        public MailMerge(ISiteMap siteMap)
            : base()
        {
            Requires.NotNull(siteMap, "config");

            _siteMap = siteMap;

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

        public MailMessage WelcomeMail(NewMemberMessage message)
        {
            ViewBag.Email = message.MemberAddress.Address;
            ViewBag.Password = message.Password;
            ViewBag.SiteUrl = _siteMap.Home().ToString();

            var mail = MailMessageX("Welcome", "_Layout");

            mail.Subject = MailResources.Welcome_Subject;
            mail.To.Add(message.MemberAddress);

            return mail;
        }

        public MailMessage NewContactNotification(NewContactMessage message)
        {
            var mail = new MailMessage {
                Body = String.Format(CultureInfo.InvariantCulture,
                    NewMessageAlertTpl, message.ContactAddress.DisplayName, message.ContactAddress.Address, message.Content),
                IsBodyHtml = false,
                Subject = String.Format(CultureInfo.InvariantCulture,
                    "Nouveau message sur le site de la part de {0}.", message.ContactAddress.Address)
            };
            mail.To.Add(Common.Constants.ContactAddress);

            return mail;
        }

        public MailMessage NewMemberNotification(NewMemberMessage message)
        {
            var mail = new MailMessage {
                Body = String.Format(CultureInfo.InvariantCulture,
                    NewMemberAlertTpl, message.MemberAddress.DisplayName, message.CompanyName, message.MemberAddress.Address),
                IsBodyHtml = false,
                Subject = String.Format(CultureInfo.InvariantCulture,
                  "Nouvelle inscription sur le site : {0}.", message.MemberAddress.DisplayName)
            };
            mail.To.Add(Common.Constants.ContactAddress);

            return mail;
        }
    }
}
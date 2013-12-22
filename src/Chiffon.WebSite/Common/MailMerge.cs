namespace Chiffon.Common
{
    using System;
    using System.Globalization;
    using System.Net.Mail;
    using Antlr4.StringTemplate;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Infrastructure.Messaging;
    using Chiffon.Properties;
    using Chiffon.Resources;
    using Narvalo;

    // TODO: 
    // * HTML
    // * Content Encoding & co
    public class MailMerge : IMailMerge
    {
        static readonly CultureInfo FrenchCultureInfo_ = new CultureInfo("fr-FR");

        readonly ISiteMap _siteMap;

        public MailMerge(ISiteMap siteMap)
        {
            Requires.NotNull(siteMap, "config");

            _siteMap = siteMap;
        }

        public MailMessage WelcomeMail(NewMemberMessage message)
        {
            // C'est un peu pourri ! À refaire ! Utiliser CultureInfo.DefaultThreadCurrentUICulture ?
            String template;
            if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "fr") {
                template = Resources.WelcomeMailBody;
            }
            else {
                template = Resources.WelcomeMailBody_English;
            }

            var tpl = new Template(template);
            tpl.Add("Message", message);
            tpl.Add("SiteUrl", _siteMap.Home().ToString());

            var mail = new MailMessage {
                Body = tpl.Render(CultureInfo.CurrentCulture),
                IsBodyHtml = false,
                Subject = MailResources.Welcome_Subject
            };
            mail.To.Add(message.EmailAddress);

            return mail;
        }

        public MailMessage NewContactNotification(NewContactMessage message)
        {
            var tpl = new Template(Resources.NewContactNotificationBody);
            tpl.Add("Message", message);

            var mail = new MailMessage {
                Body = tpl.Render(FrenchCultureInfo_),
                IsBodyHtml = false,
                Subject = String.Format(FrenchCultureInfo_,
                    "Nouveau message sur le site de la part de {0}.", message.EmailAddress.Address)
            };
            mail.To.Add(Common.Constants.ContactAddress);

            return mail;
        }

        public MailMessage NewMemberNotification(NewMemberMessage message)
        {
            var tpl = new Template(Resources.NewMemberNotificationBody);
            tpl.Add("Message", message);

            var mail = new MailMessage {
                Body = tpl.Render(FrenchCultureInfo_),
                IsBodyHtml = false,
                Subject = String.Format(FrenchCultureInfo_,
                  "Nouvelle inscription sur le site : {0}.", message.EmailAddress.DisplayName)
            };
            mail.To.Add(Common.Constants.ContactAddress);

            return mail;
        }
    }
}
namespace Chiffon.Common
{
    using System;
    using System.Globalization;
    using System.Net.Mail;

    using Antlr4.StringTemplate;
    using Chiffon.Infrastructure;
    using Chiffon.Properties;
    using Chiffon.Resources;
    using Narvalo;

    // FIXME:
    // - doit-on ajouter les "To" ici ?
    // - encodage, corps & sujet.
    // TODO: 
    // - HTML
    // - Content Encoding & co  
    // - utiliser ISO-8859-1
    // - utiliser Quoted-Printable
    // - vérifier SPF, DKIM
    // - beacon
    public sealed class MailMerge : IMailMerge
    {
        private static readonly CultureInfo FrenchCultureInfo_ = new CultureInfo("fr-FR");

        private readonly ISiteMap _siteMap;

        public MailMerge(ISiteMap siteMap)
        {
            Require.NotNull(siteMap, "config");

            _siteMap = siteMap;
        }

        public MailMessage WelcomeMail(NewMemberMessage message)
        {
            // FIXME: C'est un peu pourri ! À refaire ! Utiliser CultureInfo.DefaultThreadCurrentUICulture ?
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
            mail.To.Add(Constants.ContactAddress);

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
            mail.To.Add(Constants.ContactAddress);

            return mail;
        }

        //static void ApplyDefaultAttributes_(MailMessage message)
        //{
        //    message.BodyEncoding = Encoding.UTF8;
        //    message.BodyTransferEncoding = TransferEncoding.QuotedPrintable;
        //}
    }
}
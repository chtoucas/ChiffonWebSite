namespace Pacr.CrossCuttings.Configuration
{
    using System.Configuration;
    using Narvalo.Configuration;

    public class PacrConfigurationSection : ConfigurationSection
    {
        public const string DefaultName = "pacr";

        // Nom des propriétés.
        private const string SmtpPropertyName = "smtp";
        private const string WebSitePropertyName = "webSite";

        public static PacrConfigurationSection GetSection()
        {
            return ConfigurationSectionManager.GetSection<PacrConfigurationSection>(DefaultName);
        }

        [ConfigurationProperty(SmtpPropertyName, IsRequired = false)]
        public SmtpConfiguration Smtp
        {
            get { return (SmtpConfiguration)this[SmtpPropertyName]; }
        }

        [ConfigurationProperty(WebSitePropertyName, IsRequired = true)]
        public WebSiteSettings WebSite
        {
            get { return (WebSiteSettings)this[WebSitePropertyName]; }
        }
    }
}

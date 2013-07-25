namespace Pacr.CrossCuttings.Configuration
{
    using System.Configuration;

    public class SmtpConfiguration : ConfigurationElement
    {
        private const string HostAttributeName = "host";
        private const string PasswordAttributeName = "password";
        private const string PortAttributeName = "port";
        private const string UserNameAtrributeName = "userName";
        private const string UseSslAttributeName = "useSsl";

        [ConfigurationProperty(HostAttributeName, IsRequired = true)]
        public string Host
        {
            get { return (string)this[HostAttributeName]; }
        }

        [ConfigurationProperty(PasswordAttributeName, IsRequired = true)]
        public string Password
        {
            get { return (string)this[PasswordAttributeName]; }
        }

        [ConfigurationProperty(PortAttributeName, IsRequired = true)]
        public int Port
        {
            get { return (int)this[PortAttributeName]; }
        }

        [ConfigurationProperty(UserNameAtrributeName, IsRequired = true)]
        public string UserName
        {
            get { return (string)this[UserNameAtrributeName]; }
        }

        [ConfigurationProperty(UseSslAttributeName, DefaultValue = false, IsRequired = false)]
        public bool UseSsl
        {
            get { return (bool)this[UseSslAttributeName]; }
        }
    }
}

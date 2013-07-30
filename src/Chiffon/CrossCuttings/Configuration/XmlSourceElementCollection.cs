namespace Narvalo.Quaderno.CrossCuttings.Configuration
{
    using System;
    using System.Configuration;
    using Narvalo.Configuration;

    public class XmlSourceElementCollection
        : NamedConfigurationElementCollection<String, XmlSourceElement>
    {
        #region Champs

        const string DirectoryName_ = "directory";

        static ConfigurationProperty DirectoryProperty_
            = new ConfigurationProperty(
                DirectoryName_,
                typeof(String),
                null,
                ConfigurationPropertyOptions.None);

        string _directory;
        bool _directorySet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public XmlSourceElementCollection()
            : base("xmlSource")
        {
            _properties.Add(DirectoryProperty_);
        }

        public string Directory
        {
            get
            {
                return _directorySet ? _directory : (string)base[DirectoryProperty_];
            }
            set
            {
                Requires.NotNull(value, "value");

                _directory = value;
                _directorySet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}

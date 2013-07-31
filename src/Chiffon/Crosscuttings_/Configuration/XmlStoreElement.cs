namespace Narvalo.Quaderno.CrossCuttings.Configuration
{
    using System;
    using System.Configuration;

    public class XmlStoreElement : ConfigurationElement
    {
        #region Champs

        const string ArchivesName_ = "archives";
        const string PathName_ = "path";
        const string DirectoryName_ = "directory";

        static ConfigurationProperty ArchivesProperty_
           = new ConfigurationProperty(
               ArchivesName_, typeof(XmlSourceElementCollection), null, ConfigurationPropertyOptions.None);

        static ConfigurationProperty PathProperty_
           = new ConfigurationProperty(
               PathName_, typeof(String), null, ConfigurationPropertyOptions.IsRequired);

        static ConfigurationProperty DirectoryProperty_
           = new ConfigurationProperty(
               DirectoryName_, typeof(String), null, ConfigurationPropertyOptions.None);

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public XmlStoreElement()
            : base()
        {
            _properties.Add(ArchivesProperty_);
            _properties.Add(PathProperty_);
            _properties.Add(DirectoryProperty_);
        }

        public XmlSourceElementCollection Archives
        {
            get { return (XmlSourceElementCollection)base[ArchivesProperty_]; }
        }

        public string Directory
        {
            get { return (string)base[DirectoryProperty_]; }
        }

        public string Path
        {
            get { return (string)base[PathProperty_]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}

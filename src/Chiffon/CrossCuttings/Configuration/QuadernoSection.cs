namespace Chiffon.CrossCuttings.Configuration
{
    using System.Configuration;

    public class QuadernoSection : ConfigurationSection
    {
        #region Champs

        public const string DefaultName = "quaderno";

        const string XmlStoreName_ = "xmlStore";

        static ConfigurationProperty XmlStoreProperty_
            = new ConfigurationProperty(
                XmlStoreName_,
                typeof(XmlStoreElement),
                null,
                ConfigurationPropertyOptions.None);

        XmlStoreElement _xmlStore;
        bool _xmlStoreSet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public QuadernoSection()
            : base()
        {
            _properties.Add(XmlStoreProperty_);
        }

        public XmlStoreElement XmlStore
        {
            get
            {
                return _xmlStoreSet ? _xmlStore : (XmlStoreElement)base[XmlStoreName_];
            }
            set
            {
                Requires.NotNull(value, "value");

                _xmlStore = value;
                _xmlStoreSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}

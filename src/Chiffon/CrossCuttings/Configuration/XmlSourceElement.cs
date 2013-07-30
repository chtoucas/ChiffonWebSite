namespace Narvalo.Quaderno.CrossCuttings.Configuration
{
    using System;
    using System.Configuration;
    using Narvalo.Configuration;

    public class XmlSourceElement
        : ConfigurationElement, IKeyedConfigurationElement<String>
    {
        #region Champs

        const string EndDateName_ = "endDate";
        const string NameName_ = "name";
        const string PathName_ = "path";
        const string StartDateName_ = "startDate";

        static readonly ConfigurationProperty EndDateProperty_
            = new ConfigurationProperty(
                EndDateName_,
                typeof(DateTime),
                null,
                ConfigurationPropertyOptions.IsRequired);

        static readonly ConfigurationProperty NameProperty_
            = new ConfigurationProperty(
                NameName_,
                typeof(String),
                default(String),
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

        static readonly ConfigurationProperty PathProperty_
            = new ConfigurationProperty(
                PathName_,
                typeof(String),
                default(String),
                ConfigurationPropertyOptions.IsRequired);

        static readonly ConfigurationProperty StartDateProperty_
            = new ConfigurationProperty(
                StartDateName_,
                typeof(DateTime),
                null,
                ConfigurationPropertyOptions.IsRequired);

        DateTime _endDate;
        bool _endDateSet = false;
        string _name;
        bool _nameSet = false;
        string _path;
        bool _pathSet = false;
        DateTime _startDate;
        bool _startDateSet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public XmlSourceElement()
            : base()
        {
            _properties.Add(EndDateProperty_);
            _properties.Add(NameProperty_);
            _properties.Add(PathProperty_);
            _properties.Add(StartDateProperty_);
        }

        public string Key { get { return Name; } }

        public DateTime EndDate
        {
            get { return _endDateSet ? _endDate : (DateTime)base[EndDateProperty_]; }
            set
            {
                _endDate = value;
                _endDateSet = true;
            }
        }

        public string Name
        {
            get { return _nameSet ? _name : (string)base[NameProperty_]; }
            set
            {
                _name = value;
                _nameSet = true;
            }
        }

        public string Path
        {
            get { return _pathSet ? _path : (string)base[PathProperty_]; }
            set
            {
                _path = value;
                _pathSet = true;
            }
        }

        public DateTime StartDate
        {
            get { return _startDateSet ? _startDate : (DateTime)base[StartDateProperty_]; }
            set
            {
                _startDate = value;
                _startDateSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
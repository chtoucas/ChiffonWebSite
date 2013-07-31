namespace Pacr.CrossCuttings.Configuration
{
    using System;
    using System.Configuration;
    using Narvalo.Configuration;

    // TODO: check it is a web URI and should end with/without a trailing slash?
    public class WebSiteSettings : ConfigurationElement
    {
        private const string BaseUrlAttributeName = "baseUrl";
        //private const string MediaUrlAttributeName = "mediaUrl";
        //private const string StaticsUrlAttributeName = "staticsUrl";

        [ConfigurationProperty(BaseUrlAttributeName, IsRequired = true)]
        [AbsoluteUriValidator]
        public Uri BaseUrl
        {
            get { return new Uri((string)this[BaseUrlAttributeName]); }
        }

        //[ConfigurationProperty(MediaUrlAttributeName, IsRequired = true)]
        //[AbsoluteUriValidator]
        //public Uri StaticsUrl
        //{
        //    get { return new Uri((string)this[MediaUrlAttributeName]); }
        //}

        //[ConfigurationProperty(StaticsUrlAttributeName, IsRequired = true)]
        //[AbsoluteUriValidator]
        //public Uri MediaUrl
        //{
        //    get { return new Uri((string)this[StaticsUrlAttributeName]); }
        //}
    }
}

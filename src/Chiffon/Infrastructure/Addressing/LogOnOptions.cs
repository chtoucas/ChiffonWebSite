namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Web;

    public class LogOnOptions
    {
        public static readonly string TargetUrlKey = "target";

        Uri _targetUrl;

        public Uri TargetUrl
        {
            get { return _targetUrl; }
            set
            {
                if (_targetUrl.IsAbsoluteUri) {
                    throw ExceptionFactory.Argument("XXX", "targetUrl");
                }
                _targetUrl = value;
            }
        }

        public string ToQueryString()
        {
            var nvc = new NameValueCollection();

            if (_targetUrl != null) {
                nvc.Add(TargetUrlKey, TargetUrl.ToString());
            }

            return nvc.ToQueryString();
        }
    }

}

namespace Chiffon.Crosscuttings
{
    using System;
    using System.Web;
    using Serilog.Core;
    using Serilog.Events;

    public class HttpRequestEnricher : ILogEventEnricher
    {
        #region ILogEventEnricher

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (HttpContext.Current != null) {
                // Cela peut arriver par exemple quand on utilise trySkipIisCustomErrors.
                return;
            }

            var req = HttpContext.Current.Request;

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Domain", new ScalarValue(req.Url.Host)));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RawUrl", new ScalarValue(req.RawUrl)));
            if (req.UrlReferrer != null) {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UrlReferrer", new ScalarValue(req.UrlReferrer.ToString())));
            }
            if (!String.IsNullOrEmpty(req.UserHostAddress)) {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserHostAddress", new ScalarValue(req.UserHostAddress)));
            }
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserAgent", new ScalarValue(req.UserAgent)));
        }

        #endregion
    }
}
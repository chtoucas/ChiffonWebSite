namespace Chiffon.Common
{
    using System;
    using System.Collections;
    using System.Web;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;

    public static class HttpContextExtensions
    {
        public static ISiteMap GetSiteMap(this HttpContext context)
        {
            Requires.Object(context);

            return GetSiteMap_(context.Items);
        }

        public static ISiteMap GetSiteMap(this HttpContextBase context)
        {
            Requires.Object(context);

            return GetSiteMap_(context.Items);
        }

        public static void SetSiteMap(this HttpContext context, ISiteMap siteMap)
        {
            Requires.Object(context);
            Requires.NotNull(siteMap, "siteMap");

            context.Items["SiteMap"] = siteMap;
        }

        static ISiteMap GetSiteMap_(IDictionary dict)
        {
            var siteMap = dict["SiteMap"] as ISiteMap;
            if (siteMap == null) {
                throw new InvalidOperationException("XXX");
            }
            return siteMap;
        }
    }
}
namespace Chiffon.Common
{
    using System.Web;
    using Chiffon.Infrastructure.Addressing;

    public static class SiteMapUtility
    {
        public static ISiteMap GetSiteMap(HttpContext context)
        {
            return context.Items["SiteMap"] as ISiteMap;
        }

        public static ISiteMap GetSiteMap(HttpContextBase context)
        {
            return context.Items["SiteMap"] as ISiteMap;
        }

        public static void SetSiteMap(HttpContext context, ISiteMap siteMap)
        {
            context.Items["SiteMap"] = siteMap;
        }
    }
}
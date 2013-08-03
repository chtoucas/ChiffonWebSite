namespace Chiffon.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DesignerFilters
    {
        public static IEnumerable<Designer> WithUrlKey(this IEnumerable<Designer> list, string urlKey)
        {
            return from _ in list where _.UrlKey == urlKey select _;
        }

        public static IEnumerable<Designer> WithDesignerId(this IEnumerable<Designer> list, DesignerId designerId)
        {
            return from _ in list where _.DesignerId == designerId select _;
        }
    }
}

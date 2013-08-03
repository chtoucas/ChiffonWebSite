namespace Chiffon.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public static class PatternFilters
    {
        public static IEnumerable<Pattern> IsPublic(this IEnumerable<Pattern> list)
        {
            return from _ in list where _.IsPublic select _;
        }

        public static IEnumerable<Pattern> WithDesignerId(this IEnumerable<Pattern> list, DesignerId designerId)
        {
            return from _ in list where _.DesignerId == designerId select _;
        }
    }
}

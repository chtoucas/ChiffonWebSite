namespace Chiffon.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static int PageCount<T>(this IEnumerable<T> source, int pageSize)
        {
            return (int)Math.Ceiling((decimal)source.Count() / pageSize);
        }
    }
}
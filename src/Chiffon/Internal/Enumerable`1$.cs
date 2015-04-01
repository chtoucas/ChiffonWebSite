namespace Chiffon.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    internal static class EnumerableExtensions
    {
        public static int PageCount<T>(this IEnumerable<T> @this, int pageSize)
        {
            Contract.Requires(@this != null);

            return (int)Math.Ceiling((decimal)@this.Count() / pageSize);
        }
    }
}
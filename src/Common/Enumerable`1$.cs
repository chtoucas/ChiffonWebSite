namespace Chiffon.Common
{
    using System.Collections.Generic;
    using Narvalo;

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T element)
        {
            Requires.NotNull(source, "source");

            return AppendImpl(source, element);
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T element)
        {
            Requires.NotNull(source, "source");

            return PrependImpl(source, element);
        }

        static IEnumerable<T> PrependImpl<T>(IEnumerable<T> source, T element)
        {
            yield return element;
            foreach (var item in source) {
                yield return item;
            }
        }

        static IEnumerable<T> AppendImpl<T>(IEnumerable<T> source, T element)
        {
            foreach (var item in source) {
                yield return item;
            }
            yield return element;
        }
    }
}
namespace Chiffon.Internal
{
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    using Chiffon.Handlers;
    using Narvalo;

    internal static class CheckFor
    {
        public static class StoredProcedure
        {
            [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reader")]
            public static void Execute([ValidatedNotNull]SqlDataReader reader)
            {
                Check.NotNull(reader, "The base class 'StoredProcedure' guarantees that 'reader' is never null.");
            }
        }

        public static class HttpHandlerBase
        {
            [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context")]
            public static void ProcessRequestCore([ValidatedNotNull]HttpContext context)
            {
                Check.NotNull(context, "The base class 'HttpHandlerBase' guarantees that 'context' is never null.");
            }

            [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context")]
            [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "query")]
            public static void ProcessRequestCore<TQuery>(
                [ValidatedNotNull]HttpContext context,
                [ValidatedNotNull]TQuery query)
                where TQuery : class
            {
                Check.NotNull(context, "The base class 'HttpHandlerBase' guarantees that 'context' is never null.");
                Check.NotNull(query, "The base class 'HttpHandlerBase' guarantees that 'query' is never null.");
            }
        }
    }
}

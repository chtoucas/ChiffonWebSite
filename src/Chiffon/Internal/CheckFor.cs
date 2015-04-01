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

        public static class PatternImageHandler
        {
            [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reader")]
            public static void ProcessRequestCore([ValidatedNotNull]HttpContext context)
            {
                Check.NotNull(context, "The base class 'HttpHandlerBase' guarantees that 'context' is never null.");
            }

            [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reader")]
            public static void ProcessRequestCore([ValidatedNotNull]PatternImageQuery query)
            {
                Check.NotNull(query, "The base class 'HttpHandlerBase' guarantees that 'query' is never null.");
            }
        }
    }
}

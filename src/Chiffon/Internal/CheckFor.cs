namespace Chiffon.Internal
{
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;

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
    }
}

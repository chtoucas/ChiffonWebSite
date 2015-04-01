namespace Chiffon.Common
{
    using System.Diagnostics.Contracts;
    using System.Web;

    using Chiffon.Infrastructure;
    using Narvalo;

    public static class HttpContextExtensions
    {
        private const string HTTP_CONTEXT_KEY = "ChiffonContext";

        public static void AddChiffonContext(this HttpContext @this, ChiffonContext context)
        {
            Require.NotNull(@this, "this");

            @this.Items[HTTP_CONTEXT_KEY] = context;
        }

        public static ChiffonContext GetChiffonContext(this HttpContext @this)
        {
            Require.NotNull(@this, "this");
            Contract.Ensures(Contract.Result<ChiffonContext>() != null);

            var result = @this.Items[HTTP_CONTEXT_KEY] as ChiffonContext;

            return result ?? new ChiffonContext();
        }
    }
}

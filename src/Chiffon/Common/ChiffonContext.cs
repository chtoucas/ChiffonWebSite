namespace Chiffon.Common
{
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo;

    public sealed class ChiffonContext
    {
        private const string HTTP_CONTEXT_KEY = "ChiffonContext";

        private readonly ChiffonEnvironment _environment;

        public ChiffonContext(ChiffonEnvironment environment)
        {
            _environment = environment;
        }

        internal ChiffonContext()
            : this(ChiffonEnvironmentResolver.DefaultEnvironment) { }

        public ChiffonEnvironment Environment
        {
            get { return _environment; }
        }

        public void Register(HttpContext httpContext)
        {
            Require.NotNull(httpContext, "httpContext");

            httpContext.Items[HTTP_CONTEXT_KEY] = this;
        }

        public static ChiffonContext Resolve(HttpContext httpContext)
        {
            Require.NotNull(httpContext, "this");
            Contract.Ensures(Contract.Result<ChiffonContext>() != null);

            var result = httpContext.Items[HTTP_CONTEXT_KEY] as ChiffonContext;

            return result ?? new ChiffonContext();
        }
    }
}
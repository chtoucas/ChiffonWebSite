namespace Chiffon.Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Web;

    using Chiffon.Common;

    public sealed class ChiffonContext
    {
        private readonly ChiffonEnvironment _environment;

        public ChiffonContext(ChiffonEnvironment environment)
        {
            _environment = environment;
        }

        internal ChiffonContext()
            : this(ChiffonEnvironmentResolver.DefaultEnvironment) { }

        // Je n'aime pas utiliser HttpContext.Current !
        // Cf. http://odetocode.com/articles/112.aspx
        public static ChiffonContext Current
        {
            get
            {
                Contract.Ensures(Contract.Result<ChiffonContext>() != null);

                // TODO: Vérifier que HttpContext.Current existe bien.
                return HttpContext.Current.GetChiffonContext();
            }
        }

        public ChiffonEnvironment Environment
        {
            get { return _environment; }
        }
    }
}
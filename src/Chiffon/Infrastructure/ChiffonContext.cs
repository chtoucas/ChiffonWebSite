namespace Chiffon.Infrastructure
{
    using System.Web;
    using Narvalo;

    public class ChiffonContext
    {
        readonly ChiffonEnvironment _environment;

        internal ChiffonContext()
            : this(ChiffonEnvironment.Default) { }

        public ChiffonContext(ChiffonEnvironment environment)
        {
            Requires.NotNull(environment, "environment");

            _environment = environment;
        }

        // Je n'aime pas utiliser HttpContext.Current !
        // Cf. http://odetocode.com/articles/112.aspx
        public static ChiffonContext Current
        {
            get
            {
                // XXX: Vérifier que HttpContext.Current existe bien.
                return HttpContext.Current.GetChiffonContext();
            }
        }

        public ChiffonEnvironment Environment
        {
            get { return _environment; }
        }
    }
}
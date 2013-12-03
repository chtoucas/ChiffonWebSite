namespace Chiffon.Infrastructure
{
    using System.Threading;
    using System.Web;
    using Narvalo;

    public class ChiffonContext
    {
        static string HttpContextKey_ = "ChiffonContext";

        ChiffonEnvironment _environment;

        internal ChiffonContext(ChiffonEnvironment environment)
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
                // TODO: Lever une exception si cette valeur n'existe pas ?
                return HttpContext.Current.Items[HttpContextKey_] as ChiffonContext;
            }
        }

        public ChiffonEnvironment Environment
        {
            get { return _environment; }
            private set { _environment = value; }
        }

        internal static void Initialize(ChiffonContext context, HttpContext httpContext)
        {
            httpContext.Items[HttpContextKey_] = context;

            if (context.Environment.Language != ChiffonLanguage.Default) {
                InitializeCulture_(context.Environment.Culture);
            }
        }

        // WARNING: Cette méthode ne convient pas avec les actions asynchrones 
        // car on peut changer de Thread.
        static void InitializeCulture_(ChiffonCulture culture)
        {
            // Culture utilisée par ResourceManager.
            Thread.CurrentThread.CurrentUICulture = culture.UICulture;
            // Culture utilisée par System.Globalization.
            Thread.CurrentThread.CurrentCulture = culture.Culture;
        }
    }

}
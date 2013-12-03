namespace Chiffon.Infrastructure
{
    using System.Threading;
    using System.Web;
    using System.Web.SessionState;

    public class ChiffonContext
    {
        static string HttpContextKey_ = "ChiffonContext";

        ChiffonEnvironment _environment;

        internal ChiffonContext(ChiffonEnvironment environment)
        {
            _environment = environment;
        }

        // FIXME: Je n'aime pas utiliser HttpContext.Current
        // Cf. http://odetocode.com/articles/112.aspx
        public static ChiffonContext Current
        {
            get
            {
                return HttpContext.Current.Items[HttpContextKey_] as ChiffonContext;
            }
        }

        public ChiffonEnvironment Environment
        {
            get { return _environment; }
            private set { _environment = value; }
        }

        // NB: Cette méthode est invoquée par un module HTTP (InitializeContextModule) 
        // en tout début de requête.
        public static void Initialize(HttpContext httpContext)
        {
            var environment = ChiffonEnvironmentResolver.Resolve(httpContext.Request);

            Initialize_(environment, httpContext);
        }

        // NB: Cette méthode est invoquée par un module HTTP (InitializeVSContextModule) 
        // quand l'état de la requête ASP.NET a été acquis.
        internal static void Initialize(HttpContext httpContext, HttpSessionState session)
        {
            var environment = ChiffonEnvironmentResolver.Resolve(httpContext.Request, session);

            if (environment != null) {
                Initialize_(environment, httpContext);
            }
        }

        static void Initialize_(ChiffonEnvironment environment, HttpContext httpContext)
        {
            var context = new ChiffonContext(environment);

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
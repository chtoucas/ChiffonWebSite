namespace Chiffon.Infrastructure
{
    using System;
    using System.Threading;
    using System.Web;
    using System.Web.SessionState;

    public static class ChiffonRuntime
    {
        static object Lock_ = new Object();
        static ChiffonEnvironment Current_;

        // NB: Le seul endroit où cette propriété est utilisée est lors de la configuration d'Autofac.
        public static ChiffonEnvironment Environment
        {
            get { return Current_; }
            private set { lock (Lock_) { Current_ = value; } }
        }

        // NB: Cette méthode est invoquée par un module HTTP (InitializeRuntimeModule) en tout début de requête.
        public static void Initialize(HttpRequest request)
        {
            var environment = ChiffonEnvironmentResolver.Resolve(request);

            Initialize_(environment);
        }

        // NB: Cette méthode est invoquée par un module HTTP (InitializeRuntimeModule) quand l'état
        // de la requête ASP.NET a été acquis.
        public static void Initialize(HttpRequest request, HttpSessionState session)
        {
            var environment = ChiffonEnvironmentResolver.Resolve(request, session);

            Initialize_(environment);
        }

        static void Initialize_(ChiffonEnvironment environment)
        {
            if (environment.Language != ChiffonLanguage.Default) {
                InitializeCulture_(environment.Culture);
            }

            Environment = environment;
        }

        // WARNING: Cette méthode ne convient pas avec les actions asynchrones car on peut changer de Thread.
        static void InitializeCulture_(ChiffonCulture culture)
        {
            // Culture utilisée par ResourceManager.
            Thread.CurrentThread.CurrentUICulture = culture.UICulture;
            // Culture utilisée par System.Globalization.
            Thread.CurrentThread.CurrentCulture = culture.Culture;
        }
    }

}
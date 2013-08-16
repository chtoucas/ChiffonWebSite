﻿namespace Chiffon.Infrastructure
{
    using System;
    using System.Threading;
    using System.Web;

    internal static class ChiffonRuntime
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
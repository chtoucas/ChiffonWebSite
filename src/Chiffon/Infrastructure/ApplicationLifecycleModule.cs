namespace Chiffon.Infrastructure
{
    using System;
    using System.Net;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;
    using Narvalo.Web;
    using Serilog;

    public class ApplicationLifecycleModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.Error += OnError;
            context.Disposed += OnDisposed;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(ApplicationLifecycleModule));
        }

        /// <summary>
        /// Se produit lorsque l'application est supprimée.
        /// </summary>
        void OnDisposed(object sender, EventArgs e)
        {
            Log.Information("Application disposed.");
        }

        /// <summary>
        /// Se produit lorsqu'une exception non gérée est levée.
        /// NB: Cet événement peut être déclenché à tout moment du cycle de vie de l'application.
        /// </summary>
        void OnError(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var server = app.Server;

            var ex = server.GetLastError();
            if (ex == null) {
                // En théorie, cela ne devrait jamais se produire.
                return;
            }

            var err = HttpApplicationUtility.GetUnhandledErrorType(ex);

            switch (err) {
                case UnhandledErrorType.InvalidViewState:
                    Log.Warning(ex, ex.Message);
                    server.ClearError();
                    SetResponseStatus_(app.Response, HttpStatusCode.ServiceUnavailable);
                    break;

                case UnhandledErrorType.PotentiallyDangerousForm:
                case UnhandledErrorType.PotentiallyDangerousPath:
                    Log.Warning(ex, ex.Message);
                    server.ClearError();
                    SetResponseStatus_(app.Response, HttpStatusCode.NotFound);
                    break;

                case UnhandledErrorType.NotFound:
                    // NB: On laisse IIS prendre en charge ce type d'erreur.
                    Log.Debug(ex, ex.Message);
                    break;

                case UnhandledErrorType.Unknown:
                default:
                    Log.Fatal(ex, ex.Message);
                    break;
            }
        }

        static void SetResponseStatus_(HttpResponse response, HttpStatusCode statusCode)
        {
            if (response != null) {
                response.SetStatusCode(statusCode);
            }
        }
    }
}

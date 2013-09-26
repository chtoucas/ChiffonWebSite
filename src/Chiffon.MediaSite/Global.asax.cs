namespace Chiffon
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Routing;
    using Narvalo.Web;
    using Serilog;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Log.Information("Application starting.");

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Log.Information("Application ending.");
        }

        #region Événements

        /// <summary>
        /// Se produit lorsque l'application est supprimée.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Disposed(object sender, EventArgs e)
        {
            Log.Information("Application disposed.");
        }

        /// <summary>
        /// Se produit lorsqu'une exception non gérée est levée.
        /// NB: Application_Error peut être déclenché à tout moment du cycle de vie de l'application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
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
                    SetResponseStatus_(app, HttpStatusCode.ServiceUnavailable);
                    break;

                case UnhandledErrorType.PotentiallyDangerousForm:
                case UnhandledErrorType.PotentiallyDangerousPath:
                    Log.Warning(ex, ex.Message);
                    server.ClearError();
                    SetResponseStatus_(app, HttpStatusCode.NotFound);
                    break;

                case UnhandledErrorType.NotFound:
                    // NB: on laisse IIS prendre en charge ce type d'erreur.
                    Log.Debug(ex, ex.Message);
                    break;

                case UnhandledErrorType.Unknown:
                default:
                    Log.Fatal(ex, ex.Message);
                    break;
            }
        }

        #endregion

        static void SetResponseStatus_(HttpApplication app, HttpStatusCode statusCode)
        {
            var response = app.Response;
            if (response != null) {
                response.SetStatusCode(statusCode);
            }
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //}

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{

        //}

        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{

        //}

        //protected void Application_Error(object sender, EventArgs e)
        //{

        //}

        //protected void Session_End(object sender, EventArgs e)
        //{

        //}

        //protected void Application_End(object sender, EventArgs e)
        //{

        //}
    }
}
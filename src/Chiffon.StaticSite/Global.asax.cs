namespace Chiffon
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Routing;
    using Narvalo.Web;
    using Serilog;

    public class Global : HttpApplication
    {
        #region Méthodes spéciales.

        protected void Application_Start(object sender, EventArgs e)
        {
            Log.Information("Application starting.");

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Log.Information("Application ending.");
        }

        #endregion

        #region Événements.

        protected void Application_Disposed(object sender, EventArgs e)
        {
            Log.Information("Application disposed.");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            if (ex == null) {
                // En théorie, cela ne devrait jamais se produire.
                return;
            }

            var err = HttpApplicationUtility.GetUnhandledErrorType(ex);

            switch (err) {
                case UnhandledErrorType.InvalidViewState:
                    Log.Warning(ex, ex.Message);
                    Server.ClearError();
                    SetResponseStatus_(HttpStatusCode.ServiceUnavailable);
                    break;

                case UnhandledErrorType.PotentiallyDangerousForm:
                case UnhandledErrorType.PotentiallyDangerousPath:
                    Log.Warning(ex, ex.Message);
                    Server.ClearError();
                    SetResponseStatus_(HttpStatusCode.NotFound);
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

        void SetResponseStatus_(HttpStatusCode statusCode)
        {
            if (Response != null) {
                Response.SetStatusCode(statusCode);
            }
        }
    }
}
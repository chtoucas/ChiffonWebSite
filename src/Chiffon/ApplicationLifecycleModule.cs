namespace Chiffon
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Web;
    using System.Web.UI;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;
    using Narvalo.Web;
    using Serilog;

    public sealed class ApplicationLifecycleModule : IHttpModule
    {
        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(ApplicationLifecycleModule));
        }

        public void Init(HttpApplication context)
        {
            Require.NotNull(context, "context");

            context.Error += OnError_;
            context.Disposed += OnDisposed_;
            context.PreSendRequestHeaders += OnPreSendRequestHeaders_;
        }

        public void Dispose() { }

        /// <summary>
        /// Ajoute des en-têtes de réponse facultatives mais qui peuvent améliorer
        /// la sécurité de l'application.
        /// </summary>
        /// <remarks>
        /// Toutes ces en-têtes pourraient être rajoutées via "Web.config".
        /// </remarks>
        /// <param name="headers">Collection d'en-têtes de réponse.</param>
        private static void AddSecurityHeaders_(NameValueCollection headers)
        {
            Contract.Requires(headers != null);

            // Cf. http://www.html5rocks.com/en/tutorials/security/content-security-policy/
            //headers.Add("Content-Security-Policy", "");

            // Habituellement on essaie de prévenir l'inclusion d'une page du site dans une "frame"
            // via javascript. Malheureusement cela ne suffit pas et il existe toujours des
            // solutions pour contrer les techniques basées sur javascript.
            // Cf. http://en.wikipedia.org/wiki/Framekiller
            headers.Add("X-Frame-Options", "DENY");
        }

        private static HttpStatusCode GetStatusCode_(Exception exception)
        {
            Contract.Requires(exception != null);

            Type type = exception.GetType();
            var httpException = exception as HttpException;

            // Lorsqu'une exception de type ViewStateException ou HttpRequestValidationException
            // est levée, ASP.NET retourne une erreur HTTP 500, on préfère utiliser une erreur
            // HTTP 400.
            if (httpException == null)
            {
                return type == typeof(ViewStateException)
                     ? HttpStatusCode.BadRequest
                     : HttpStatusCode.InternalServerError;
            }
            else
            {
                return type == typeof(HttpRequestValidationException)
                    ? HttpStatusCode.BadRequest
                    : (HttpStatusCode)httpException.GetHttpCode();
            }
        }

        /// <summary>
        /// Si on ne fait pas gaffe, IIS et ASP.NET créent de nombreuses en-têtes inutiles :
        /// - Server, rajouté par IIS ;
        /// - X-AspNet-Version ;
        /// - X-AspNetMvc-Version, rajouté par System.Web.Mvc.MvcHandler ;
        /// - X-AspNetWebPages-Version, rajouté par System.Web.WebPages.WebPageHttpHandler ;
        /// - X-Powered-By, rajouté par IIS ;
        /// - X-SourceFiles, générée par Visual Studio, que nous pouvons donc ignorer.
        /// Cette méthode supprime l'en-tête "Server".
        ///
        /// Pour une discution détaillée, se référer à
        /// <see cref="!:http://www.troyhunt.com/2012/02/shhh-dont-let-your-response-headers.html"/>
        /// </summary>
        /// <remarks>
        /// Pour supprimer l'en-tête " X-AspNet-Version", modifier "Web.config" comme suit :
        /// <code>
        /// &lt;system.web&gt;
        ///   &lt;httpRuntime enableVersionHeader="false" /&gt;
        /// &lt;/system.web&gt;
        /// </code>
        ///
        /// Pour supprimer l'en-tête "X-AspNetMvc-Version", ajouter à "Application_Start"
        /// la ligne suivante :
        /// <code>
        /// MvcHandler.DisableMvcResponseHeader = true;
        /// </code>
        ///
        /// Pour supprimer l'en-tête "X-AspNetWebPages-Version", ajouter à "Application_Start"
        /// la ligne suivante :
        /// <code>
        /// WebPageHttpHandler.DisableWebPagesResponseHeader = true;
        /// </code>
        /// Cette en-tête n'est générée que pour les vues accédées directement.
        ///
        /// Pour supprimer l'en-tête "X-Powered-By", modifier "Web.config" comme suit :
        /// <code>
        ///  &lt;system.webServer&gt;
        ///    &lt;httpProtocol&gt;
        ///      &lt;customHeaders&gt;
        ///        &lt;clear /&gt;
        ///        &lt;remove name="X-Powered-By" /&gt;
        ///      &lt;/customHeaders&gt;
        ///    &lt;/httpProtocol&gt;
        ///  &lt;/system.webServer&gt;
        /// </code>
        /// Ce n'est pas la peine d'essayer de supprimer cette en-tête en manipulant directement
        /// la collection <c>response.Headers</c>, car cela n'aura aucun effet.
        /// On peut aussi configurer IIS pour qu'il n'émette pas cette en-tête.
        /// </remarks>
        /// <param name="headers">Collection d'en-têtes de réponse.</param>
        private static void RemoveUnnecessaryHeaders_(NameValueCollection headers)
        {
            Contract.Requires(headers != null);

            headers.Remove("Server");
        }

        /// <summary>
        /// Se produit lorsque l'application est supprimée.
        /// </summary>
        private void OnDisposed_(object sender, EventArgs e)
        {
            Log.Information("Application disposed.");
        }

        /// <summary>
        /// Se produit lorsqu'une exception non gérée est levée.
        /// NB: Cet événement peut être déclenché à tout moment du cycle de vie de l'application.
        /// </summary>
        private void OnError_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var server = app.Server;

            var ex = server.GetLastError();
            if (ex == null)
            {
                Log.Fatal("An unhandled error occurred but no exception found.");
                return;
            }

            var statusCode = GetStatusCode_(ex);

            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    Log.Warning(ex, ex.Message);
                    server.ClearError();
                    app.Response.SetStatusCode(statusCode);
                    break;
                case HttpStatusCode.NotFound:
                    Log.Debug(ex, ex.Message);
                    break;
                default:
                    Log.Fatal(ex, ex.Message);
                    break;
            }
        }

        private void OnPreSendRequestHeaders_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var response = app.Response;
            if (response == null)
            {
                // Peut arriver si "trySkipIisCustomErrors" est égal à "true".
                return;
            }

            NameValueCollection headers = response.Headers;

            RemoveUnnecessaryHeaders_(headers);
            AddSecurityHeaders_(headers);
        }
    }
}

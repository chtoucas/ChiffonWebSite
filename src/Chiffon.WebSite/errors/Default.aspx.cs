namespace Chiffon.Errors
{
    using System;
    using System.Web;
    using System.Web.UI;
    using Chiffon.Properties;

    public partial class DefaultPage : Page
    {
        string _errorMessage = VR.Error_DefaultMessage;

        public DefaultPage()
            : base()
        {
            Load += Page_Load;
        }

        protected string ErrorMessage { get { return _errorMessage; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            // TODO: Peut-on plutôt utiliser Context.Error au lieu de Server.GetLastError() ?
            var ex = Server.GetLastError() as HttpException;
            if (ex == null)
            {
                // En théorie, cela ne devrait jamais se produire.
                // Cependant, cela dépend du mode d'exécution de cette page et il semble bien
                // que ASP.NET ne garde pas l'erreur d'origine.
                return;
            }

            var statusCode = ex.GetHttpCode();

            if (statusCode >= 500)
            {
                _errorMessage = VR.Error_InternalServerError;
            }
            else if (statusCode >= 400)
            {
                _errorMessage = VR.Error_BadRequest;
            }

            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = statusCode;
        }
    }
}
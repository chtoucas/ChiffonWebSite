namespace Chiffon.Common
{
    using System;
    using System.Web.UI;

    public class ErrorPage : Page
    {
        readonly int _statusCode;

        public ErrorPage(int statusCode)
        {
            _statusCode = statusCode;

            this.Load += Page_Load;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = _statusCode;

            Page_LoadCore(sender, e);
        }

        protected virtual void Page_LoadCore(object sender, EventArgs e)
        {
            ;
        }
    }
}
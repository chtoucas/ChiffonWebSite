namespace Chiffon.Common
{
    using System;
    using System.Web.UI;
    using Chiffon.Errors;

    public class ErrorPage : Page
    {
        readonly int _statusCode;
        ErrorMasterPage _errorMaster;

        public ErrorPage(int statusCode)
        {
            _statusCode = statusCode;

            this.Init += Page_Init;
            this.Load += Page_Load;
        }

        protected ErrorMasterPage ErrorMaster { get { return _errorMaster; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = _statusCode;

            Page_LoadCore(sender, e);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            var master = Master as ErrorMasterPage;
            if (master == null) {
                throw new InvalidOperationException("Your ErrorPage does not use ErrorMasterPage.");
            }
            _errorMaster = master;
        }

        protected virtual void Page_LoadCore(object sender, EventArgs e)
        {
            ;
        }
    }
}
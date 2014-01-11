namespace Chiffon.Errors
{
    using System;
    using System.Net;
    using Chiffon.Common;

    public partial class ServiceUnavailablePage : ErrorPageBase
    {
        public ServiceUnavailablePage() : base() { }

        protected override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.ServiceUnavailable; }
        }

        protected override void Page_LoadCore(object sender, EventArgs e)
        {
            // On peut réessayer dans 1H.
            Response.Headers.Add("Retry-After", "3600");
        }
    }
}
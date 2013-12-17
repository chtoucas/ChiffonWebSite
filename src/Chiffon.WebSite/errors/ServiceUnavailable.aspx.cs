namespace Chiffon.Errors
{
    using System;
    using System.Net;
    using Chiffon.Common;

    public partial class ServiceUnavailablePage : ErrorPage
    {
        public ServiceUnavailablePage() : base((int)HttpStatusCode.ServiceUnavailable) { }

        protected override void Page_LoadCore(object sender, EventArgs e)
        {
            // On instruit principalement Googlebot de réessayer dans 1H.
            Response.Headers.Add("Retry-After", "3600");
        }
    }
}
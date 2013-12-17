namespace Chiffon.Errors
{
    using System;
    using Chiffon.Common;

    public partial class ServiceUnavailablePage : ErrorPage
    {
        public ServiceUnavailablePage() : base(503) { }

        protected override void Page_LoadCore(object sender, EventArgs e)
        {
            // On instruit principalement Googlebot de réessayer dans 1H.
            Response.Headers.Add("Retry-After", "3600");
        }
    }
}
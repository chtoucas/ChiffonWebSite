namespace Chiffon.Errors
{
    using System.Net;
    using Chiffon.Common;

    public partial class BadRequestPage : ErrorPage
    {
        public BadRequestPage() : base((int)HttpStatusCode.BadRequest) { }

        protected override void Page_LoadCore(object sender, System.EventArgs e)
        {
            base.Page_LoadCore(sender, e);

            ErrorMaster.ShowHomeLink();
        }
    }
}
namespace Chiffon.Errors
{
    using System.Net;
    using Chiffon.Common;

    public partial class NotFoundPage : ErrorPage
    {
        public NotFoundPage() : base((int)HttpStatusCode.NotFound) { }

        protected override void Page_LoadCore(object sender, System.EventArgs e)
        {
            base.Page_LoadCore(sender, e);

            ErrorMaster.ShowHomeLink();
        }
    }
}
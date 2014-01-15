namespace Chiffon.Errors
{
    using System.Net;
    using Narvalo.Web;

    public partial class InternalServerErrorPage : ErrorPageBase
    {
        public InternalServerErrorPage() : base() { }

        protected override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.InternalServerError; }
        }
    }
}
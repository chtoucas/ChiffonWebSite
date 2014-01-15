namespace Chiffon.Errors
{
    using System.Net;
    using Narvalo.Web;

    public partial class BadRequestPage : ErrorPageBase
    {
        public BadRequestPage() : base() { }

        protected override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.BadRequest; }
        }
    }
}
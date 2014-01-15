namespace Chiffon.Errors
{
    using System.Net;
    using Narvalo.Web;

    public partial class NotFoundPage : ErrorPageBase
    {
        public NotFoundPage() : base() { }

        protected override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.NotFound; }
        }
    }
}
namespace Chiffon.Errors
{
    using System.Net;
    using Chiffon.Common;

    public partial class NotFoundPage : ErrorPageBase
    {
        public NotFoundPage() : base() { }

        protected override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.NotFound; }
        }
    }
}
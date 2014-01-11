namespace Chiffon.Errors
{
    using System.Net;
    using Chiffon.Common;

    public partial class InternalServerErrorPage : ErrorPageBase
    {
        public InternalServerErrorPage() : base() { }

        protected override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.InternalServerError; }
        }
    }
}
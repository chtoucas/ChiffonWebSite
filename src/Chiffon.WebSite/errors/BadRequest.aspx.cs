namespace Chiffon.Errors
{
    using System.Net;
    using Chiffon.Common;

    public partial class BadRequestPage : ErrorPage
    {
        public BadRequestPage() : base((int)HttpStatusCode.BadRequest) { }
    }
}
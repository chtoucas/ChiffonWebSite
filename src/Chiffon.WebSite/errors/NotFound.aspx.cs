namespace Chiffon.Errors
{
    using System.Net;
    using Chiffon.Common;

    public partial class NotFoundPage : ErrorPage
    {
        public NotFoundPage() : base((int)HttpStatusCode.NotFound) { }
    }
}
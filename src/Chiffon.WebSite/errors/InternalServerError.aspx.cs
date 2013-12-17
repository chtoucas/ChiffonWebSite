namespace Chiffon.Errors
{
    using System.Net;
    using Chiffon.Common;

    public partial class InternalServerErrorPage : ErrorPage
    {
        public InternalServerErrorPage() : base((int)HttpStatusCode.InternalServerError) { }
    }
}
namespace Mvc.Mailer
{
    using System.Web;

    public class EmptyHttpContext : HttpContextBase
    {
        public override HttpRequestBase Request
        {
            get
            {
                return new HttpRequestWrapper(new HttpRequest("", "", ""));
            }
        }
    }
}

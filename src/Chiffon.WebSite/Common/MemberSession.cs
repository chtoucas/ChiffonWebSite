namespace Chiffon.Common
{
    using System.Web;
    using Chiffon.Entities;

    // FIXME: AppPool recycling.
    public class MemberSession
    {
        const string HttpSessionKey = "member";

        readonly HttpContextBase _httpContext;

        public MemberSession(HttpContext httpContext) : this(new HttpContextWrapper(httpContext)) { }

        public MemberSession(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        protected HttpSessionStateBase Session
        {
            get { return _httpContext.Session; }
        }

        public Member Value
        {
            get { return Session[HttpSessionKey] as Member; }
            set { Session[HttpSessionKey] = value; }
        }

        public void Clear()
        {
            Session.Clear();
        }
    }
}
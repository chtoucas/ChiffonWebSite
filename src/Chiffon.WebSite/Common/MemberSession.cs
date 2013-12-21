namespace Chiffon.Common
{
    using System;
    using System.Web;

    // FIXME: AppPool recycling.
    public class MemberSession
    {
        const string HttpSessionKey = "memberInfo";

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

        public string EmailAddress
        {
            get { return Session[HttpSessionKey] as String; }
            set { Session[HttpSessionKey] = value; }
        }

        public void Clear()
        {
            Session.Clear();
        }
    }
}
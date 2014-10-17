namespace Chiffon.Common
{
    using System.Web;
    using System.Web.Security;
    using Chiffon.Entities;
    using Narvalo;

    public class AuthentificationService
    {
        MemberSession _memberSession;

        public AuthentificationService(HttpContextBase httpContext)
        {
            Require.NotNull(httpContext, "httpContext");

            _memberSession = new MemberSession(httpContext.Session);
        }

        public AuthentificationService(HttpContext httpContext) : this(new HttpContextWrapper(httpContext)) { }

        public void SignIn(Member member)
        {
            Require.NotNull(member, "member");

            FormsAuthentication.SetAuthCookie(member.DisplayName, false /* createPersistentCookie */);
            _memberSession.Value = member;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            _memberSession.Clear();
        }
    }

}
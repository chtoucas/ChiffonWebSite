namespace Chiffon.Common
{
    using System.Web;

    using Chiffon.Entities;

    // WARNING: Pour le moment, si l'AppPool est recyclé on perd la session car on utilise
    // le mode de fonctionnement "InProc".
    public class MemberSession
    {
        private const string HTTP_SESSION_KEY = "member";

        private readonly HttpSessionStateBase _httpSession;

        public MemberSession(HttpSessionStateBase httpSession)
        {
            _httpSession = httpSession;
        }

        public Member Value
        {
            get { return _httpSession[HTTP_SESSION_KEY] as Member; }
            set { _httpSession[HTTP_SESSION_KEY] = value; }
        }

        public void Clear()
        {
            _httpSession.Clear();
        }
    }
}
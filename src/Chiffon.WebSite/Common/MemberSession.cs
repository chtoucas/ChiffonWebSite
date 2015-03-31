﻿namespace Chiffon.Common
{
    using System.Web;
    using Chiffon.Entities;

    // WARNING: Pour le moment, si l'AppPool est recyclé on perd la session car on utilise
    // le mode de fonctionnement "InProc".
    public class MemberSession
    {
        private const string HttpSessionKey = "member";

        private readonly HttpSessionStateBase _httpSession;

        public MemberSession(HttpSessionStateBase httpSession)
        {
            _httpSession = httpSession;
        }

        public Member Value
        {
            get { return _httpSession[HttpSessionKey] as Member; }
            set { _httpSession[HttpSessionKey] = value; }
        }

        public void Clear()
        {
            _httpSession.Clear();
        }
    }
}
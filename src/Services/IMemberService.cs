namespace Chiffon.Services
{
    using System;
    using Narvalo.Fx;

    public interface IMemberService
    {
        bool LogOn(string token, bool createPersistentCookie);
    }
}

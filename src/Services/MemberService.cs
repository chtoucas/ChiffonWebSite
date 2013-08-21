namespace Chiffon.Services
{
    using Narvalo;
    using Narvalo.Web.Security;

    public class MemberService/*Impl*/ : IMemberService
    {
        readonly IFormsAuthenticationService _formsService;

        public MemberService(IFormsAuthenticationService formsService)
        {
            Requires.NotNull(formsService, "formsService");

            _formsService = formsService;
        }

        #region IMemberService

        public bool LogOn(string token, bool createPersistentCookie)
        {
            // TODO: Enregistrer l'événement avec context.Request.UserHostAddress.

            // FIXME
            var succeed = true;
            var userName = "XXX";

            if (succeed) {
                _formsService.SignIn(userName, createPersistentCookie);
            }

            return succeed;
        }

        #endregion
    }
}
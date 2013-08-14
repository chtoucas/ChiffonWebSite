namespace Chiffon.Services
{
    using Narvalo;
    using Narvalo.Web.Security;

    public class MemberService/*Impl*/ : IMemberService
    {
        readonly IFormsAuthenticationService _authentificationService;

        public MemberService(IFormsAuthenticationService authentificationService)
        {
            Requires.NotNull(authentificationService, "authentificationService");

            _authentificationService = authentificationService;
        }

        #region IMemberService

        public bool LogOn(string token, bool createPersistentCookie)
        {
            // TODO: Enregistrer l'événement avec context.Request.UserHostAddress.

            // FIXME
            var succeed = true;
            var userName = "XXX";

            if (succeed) {
                _authentificationService.SignIn(userName, createPersistentCookie);
            }

            return succeed;
        }

        #endregion
    }
}
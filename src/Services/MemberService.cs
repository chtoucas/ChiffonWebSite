namespace Chiffon.Services
{
    public class MemberService/*Impl*/ : IMemberService
    {
        #region IMemberService

        // FIXME: c'est un peu laxiste et franchement dangereux...
        public string LogOn(string token)
        {
            // TODO: Enregistrer l'événement avec context.Request.UserHostAddress.

            return "XXX";
        }

        #endregion
    }
}
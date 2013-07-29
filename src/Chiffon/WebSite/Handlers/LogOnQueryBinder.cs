namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.Web;
    using Chiffon.WebSite.Resources;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class LogOnQueryBinder
    {
        public LogOnQueryBinder() { }

        public Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            var form = request.Form;

            // > Paramètres obligatoires <

            //var emailAddress = form.MayGetValue(LogOnQuery.EmailAddressKey)
            //    .Match(
            //        _ => MailAddressUtility.Create(_),
            //        () => Outcome<MailAddress>.Failure(LogOnModelResources.EmailAddressIsRequired)
            //    );
            //if (emailAddress.Unsuccessful) { return emailAddress.Forget<LogOnQuery>(); }

            var password = form.MayGetValue(LogOnQuery.PasswordKey);
            if (password.IsNone) { return Outcome<LogOnQuery>.Failure(Error.Create(SR.LogOnModel_PasswordIsRequired)); }

            // > Paramètres optionnels <

            var targetUrl = form.MayParseValue(
                LogOnQuery.TargetUrlKey, _ => MayParse.ToUri(_, UriKind.Relative));
            if (targetUrl.IsNone) { return Outcome<LogOnQuery>.Failure(Error.Create(SR.LogOnModel_TargetUrlIsNotUri)); }

            var persistent = form.MayParseValue(
                LogOnQuery.CreatePersistentCookieKey, _ => MayParse.ToBoolean(_, BooleanStyles.Any));
            if (persistent.IsNone) { return Outcome<LogOnQuery>.Failure(Error.Create(SR.LogOnModel_PersistentIsNotBoolean)); }

            // > Création du modèle <

            var model = new LogOnQuery {
                //EmailAddress = emailAddress.Value,
                Password = password.Value
            };
            targetUrl.WhenSome(_ => { model.TargetUrl = _; });
            persistent.WhenSome(_ => { model.CreatePersistentCookie = _; });

            return Outcome<LogOnQuery>.Success(model);
        }
    }
}

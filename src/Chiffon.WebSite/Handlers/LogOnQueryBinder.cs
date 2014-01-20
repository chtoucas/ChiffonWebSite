namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Linq;
    using Narvalo.Web;

    public class LogOnQueryBinder : IQueryBinder<LogOnQuery>
    {
        Maybe<string> Email { get; set; }
        Maybe<string> Password { get; set; }

        public bool CanValidate { get; private set; }

        public LogOnQuery Bind(HttpRequest request)
        {
            Require.NotNull(request, "request");

            var result = new LogOnQuery();

            var form = request.Form;

            Email = form.MayGetValue("email").OnSome(_ => result.Email = _);
            Password = form.MayGetValue("password").OnSome(_ => result.Password = _);

            result.TargetUrl = form.MayGetValue("targeturl")
                 .Bind(_ => MayCreate.Uri(_, UriKind.Relative));

            CanValidate = true;

            return result;
        }

        public bool Validate()
        {
            if (!CanValidate) {
                throw new InvalidOperationException("FIXME");
            }

            return Email.Filter(_ => _.Length > 0).IsNone
                || Password.Filter(_ => _.Length > 0).IsNone;
        }
    }
}

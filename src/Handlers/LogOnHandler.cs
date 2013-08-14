namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Common;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;
    using Narvalo.Web.Security;

    public class LogOnHandler : HttpHandlerBase<LogOnQuery>, IRequiresSessionState
    {
        readonly IMemberService _memberService;
        readonly IFormsAuthenticationService _formsService;

        public LogOnHandler(IMemberService memberService, IFormsAuthenticationService formsService)
            : base()
        {
            _memberService = memberService;
            _formsService = formsService;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            var form = request.Form;

            var token = form.MayGetValue("token").Filter(_ => _.Length > 0);
            if (token.IsNone) { return BindingFailure("token"); }

            var targetUrl = form.MayGetValue("targeturl").Bind(_ => MayParse.ToUri(_, UriKind.Relative));

            var model = new LogOnQuery { TargetUrl = targetUrl, Token = token.Value };

            return Outcome<LogOnQuery>.Success(model);
        }

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            var succeed = _memberService.LogOn(query.Token, false /* createPersistentCookie */);

            if (succeed) {
                //_formsService.SignIn(
            }

            // XXX: Pas sûr que ne pas utiliser l'IoC pour le SiteMap soit une bonne idée.
            var siteMap = SiteMapUtility.GetSiteMap(context);

            Uri nextUrl = succeed
                ? query.TargetUrl.Match(_ => siteMap.MakeAbsoluteUri(_), siteMap.Home())
                : query.TargetUrl.Match(_ => siteMap.LogOn(_), siteMap.LogOn());

            context.Response.Redirect(nextUrl.ToString());
        }
    }
}

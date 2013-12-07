namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
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
        readonly ISiteMapFactory _siteMapFactory;

        public LogOnHandler(
            IMemberService memberService, 
            IFormsAuthenticationService formsService,
            ISiteMapFactory siteMapFactory)
            : base()
        {
            Requires.NotNull(memberService, "memberService");
            Requires.NotNull(formsService, "formsService");
            Requires.NotNull(siteMapFactory, "siteMapFactory");

            _memberService = memberService;
            _formsService = formsService;
            _siteMapFactory = siteMapFactory;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            Requires.NotNull(request, "request");

            var form = request.Form;

            var token = form.MayGetValue("token").Filter(_ => _.Length > 0);
            if (token.IsNone) { return BindingFailure("token"); }

            var targetUrl = form.MayGetValue("targeturl").Bind(_ => MayParse.ToUri(_, UriKind.Relative));

            var model = new LogOnQuery { TargetUrl = targetUrl, Token = token.Value };

            return Outcome<LogOnQuery>.Success(model);
        }

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            Requires.NotNull(context, "context");
            Requires.NotNull(query, "query");

            var userName = _memberService.LogOn(query.Token);

            var succeed = !String.IsNullOrEmpty(userName);
            if (succeed) {
                _formsService.SignIn(userName, false /* createPersistentCookie */);
            }

            var siteMap = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment);

            Uri nextUrl = succeed
                ? query.TargetUrl.Match(_ => siteMap.MakeAbsoluteUri(_), siteMap.Home())
                : query.TargetUrl.Match(_ => siteMap.LogOn(_), siteMap.LogOn());

            context.Response.Redirect(nextUrl.ToString());
        }
    }
}

namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class LogOnHandler : HttpHandlerBase<LogOnQuery>, IRequiresSessionState
    {
        readonly IMemberService _memberService;
        readonly ISiteMapFactory _siteMapFactory;

        public LogOnHandler(IMemberService memberService, ISiteMapFactory siteMapFactory)
            : base()
        {
            Requires.NotNull(memberService, "memberService");
            Requires.NotNull(siteMapFactory, "siteMapFactory");

            _memberService = memberService;
            _siteMapFactory = siteMapFactory;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            Requires.NotNull(request, "request");

            var form = request.Form;

            var email = form.MayGetValue("email").Filter(_ => _.Length > 0);
            if (email.IsNone) { return BindingFailure("email"); }

            var password = form.MayGetValue("password").Filter(_ => _.Length > 0);
            if (password.IsNone) { return BindingFailure("password"); }

            var targetUrl = form.MayGetValue("targeturl").Bind(_ => MayParse.ToUri(_, UriKind.Relative));

            var model = new LogOnQuery {
                Email = email.Value,
                Password = password.Value,
                TargetUrl = targetUrl
            };

            return Outcome.Success(model);
        }

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            Requires.NotNull(context, "context");
            Requires.NotNull(query, "query");

            var result = _memberService.MayLogOn(query.Email, query.Password);

            result.WhenSome(_ => { (new AuthentificationService(context)).SignIn(_); });

            var siteMap = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment);

            Uri nextUrl = result.IsSome
                ? query.TargetUrl.Match(_ => siteMap.MakeAbsoluteUri(_), siteMap.Home())
                : query.TargetUrl.Match(_ => siteMap.LogOn(_), siteMap.LogOn());

            context.Response.Redirect(nextUrl.ToString());
        }
    }
}

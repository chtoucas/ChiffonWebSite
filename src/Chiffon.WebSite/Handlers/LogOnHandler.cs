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
    using Narvalo.Fx;
    using Narvalo.Linq;
    using Narvalo.Web;

    // TODO: ValidateAntiForgeryToken.
    public class LogOnHandler : HttpHandlerBase<LogOnQuery>, IRequiresSessionState
    {
        readonly IMemberService _memberService;
        readonly ISiteMapFactory _siteMapFactory;

        public LogOnHandler(IMemberService memberService, ISiteMapFactory siteMapFactory)
            : base()
        {
            Require.NotNull(memberService, "memberService");
            Require.NotNull(siteMapFactory, "siteMapFactory");

            _memberService = memberService;
            _siteMapFactory = siteMapFactory;
        }

        public override bool IsReusable { get { return true; } }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            DebugCheck.NotNull(request);

            var form = request.Form;

            var email = form.MayGetValue("email").Filter(_ => _.Length > 0);
            if (email.IsNone) { return CreateFailure("email"); }

            var password = form.MayGetValue("password").Filter(_ => _.Length > 0);
            if (password.IsNone) { return CreateFailure("password"); }

            var targetUrl = form.MayGetValue("targeturl").Bind(_ => MayCreate.Uri(_, UriKind.Relative));

            var model = new LogOnQuery {
                Email = email.Value,
                Password = password.Value,
                TargetUrl = targetUrl
            };

            return Outcome.Create(model);
        }

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            DebugCheck.NotNull(context);
            DebugCheck.NotNull(query);

            var siteMap = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment);

            var nextUrl = _memberService
                .MayLogOn(query.Email, query.Password)
                .OnSome(_ => (new AuthentificationService(context)).SignIn(_))
                .Match(
                    _ => GetNextUri_(query.TargetUrl, siteMap),
                    () => GetLoginUri_(query.TargetUrl, siteMap));

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }

        Uri GetNextUri_(Maybe<Uri> targetUrl, ISiteMap siteMap)
        {
            return targetUrl.Match(_ => siteMap.MakeAbsoluteUri(_), siteMap.Home());
        }

        Uri GetLoginUri_(Maybe<Uri> targetUrl, ISiteMap siteMap)
        {
            return targetUrl.Match(_ => siteMap.Login(_), siteMap.Login());
        }
    }
}

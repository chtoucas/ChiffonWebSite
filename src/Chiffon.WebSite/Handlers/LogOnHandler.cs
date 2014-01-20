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

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            Require.NotNull(request, "request");

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
            Require.NotNull(context, "context");
            Require.NotNull(query, "query");

            var result = _memberService.MayLogOn(query.Email, query.Password);

            var siteMap = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment);

            Uri nextUrl = null;

            result.OnSome(_ =>
            {
                (new AuthentificationService(context)).SignIn(_);
                nextUrl = query.TargetUrl.Match(value => siteMap.MakeAbsoluteUri(value), siteMap.Home());
            }).OnNone(() =>
            {
                nextUrl = query.TargetUrl.Match(_ => siteMap.Login(_), siteMap.Login());
            });

            //Uri nextUrl = result.IsSome
            //    ? query.TargetUrl.Match(_ => siteMap.MakeAbsoluteUri(_), siteMap.Home())
            //    : query.TargetUrl.Match(_ => siteMap.Login(_), siteMap.Login());

            context.Response.Redirect(nextUrl.ToString());
        }
    }
}

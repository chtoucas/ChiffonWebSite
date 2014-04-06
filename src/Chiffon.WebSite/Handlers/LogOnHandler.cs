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
    using Narvalo.Web;

    // TODO: ValidateAntiForgeryToken.
    public class LogOnHandler
        : HttpHandlerBase<LogOnQuery, LogOnQueryBinder>, IRequiresSessionState
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

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            DebugCheck.NotNull(context);
            DebugCheck.NotNull(query);

            var siteMap = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment);

            var nextUrl = _memberService
                .MayLogOn(query.Email, query.Password)
                .Run(_ => (new AuthentificationService(context)).SignIn(_))
                .Select(_ => GetNextUri_(query.TargetUrl, siteMap))
                .ValueOrElse(GetLoginUri_(query.TargetUrl, siteMap));

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }

        Uri GetNextUri_(Maybe<Uri> targetUrl, ISiteMap siteMap)
        {
            return targetUrl.Select(_ => siteMap.MakeAbsoluteUri(_)).ValueOrElse(siteMap.Home());
        }

        Uri GetLoginUri_(Maybe<Uri> targetUrl, ISiteMap siteMap)
        {
            return targetUrl.Select(_ => siteMap.Login(_)).ValueOrElse(siteMap.Login());
        }
    }
}

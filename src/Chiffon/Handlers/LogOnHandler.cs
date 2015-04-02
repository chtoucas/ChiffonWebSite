namespace Chiffon.Handlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using Chiffon.Common;
    using Chiffon.Internal;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Web;

    // TODO: ValidateAntiForgeryToken.
    public sealed class LogOnHandler
        : HttpHandlerBase<LogOnQuery, LogOnQueryBinder>, IRequiresSessionState
    {
        private readonly IMemberService _memberService;
        private readonly ISiteMapFactory _siteMapFactory;

        public LogOnHandler(IMemberService memberService, ISiteMapFactory siteMapFactory)
            : base()
        {
            Require.NotNull(memberService, "memberService");
            Require.NotNull(siteMapFactory, "siteMapFactory");

            _memberService = memberService;
            _siteMapFactory = siteMapFactory;

            IsReusable = true;
        }

        public override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            CheckFor.HttpHandlerBase.ProcessRequestCore(context, query);

            var environment = ChiffonContext.Resolve(HttpContext.Current).Environment;
            var siteMap = _siteMapFactory.CreateMap(environment);

            var member = _memberService
                .MayLogOn(query.Email, query.Password);

            member.OnSome(_ => new AuthenticationService(context).SignIn(_));

            var nextUrl = member
                .Select(_ => GetNextUri_(query.TargetUrl, siteMap, environment))
                .ValueOrElse(GetLoginUri_(query.TargetUrl, siteMap));

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }

        private static Uri GetNextUri_(Maybe<Uri> targetUrl, ISiteMap siteMap, ChiffonEnvironment environment)
        {
            Contract.Requires(targetUrl != null);
            Contract.Requires(siteMap != null);

            return targetUrl.Select(_ => environment.MakeAbsoluteUri(_)).ValueOrElse(siteMap.Home());
        }

        private static Uri GetLoginUri_(Maybe<Uri> targetUrl, ISiteMap siteMap)
        {
            Contract.Requires(targetUrl != null);
            Contract.Requires(siteMap != null);

            return targetUrl.Select(_ => siteMap.Login(_)).ValueOrElse(siteMap.Login());
        }
    }
}

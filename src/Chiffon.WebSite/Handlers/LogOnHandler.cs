namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Narvalo.Fx;
    using Narvalo.Web;
    //using Pacr.BuildingBlocks.Membership;
    //using Pacr.Infrastructure.Addressing;

    public class LogOnHandler : HttpHandlerBase<LogOnQuery>, IRequiresSessionState
    {
        readonly IMemberService _memberService;

        public LogOnHandler(IMemberService memberService)
            : base()
        {
            _memberService = memberService;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            return new LogOnQueryBinder().Bind(request);
        }

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            // TODO: loguer context.Request.UserHostAddress.
            // TODO: cookie persistent.
            MemberId memberId = _memberService.LogOn(query.EmailAddress, query.Password);

            var map = SiteMapBuilder.Current.GetSiteMapFactory().CreateMap(memberId);

            Uri nextUrl
                = memberId.IsAnonymous
                ? map.LogOn(query)
                : query.TargetUrl != null
                    ? map.MakeAbsoluteUri(query.TargetUrl)
                    : map.Home();

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }
    }
}

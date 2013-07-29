namespace Chiffon.WebSite.HttpHandlers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Narvalo.Web;
    using Pacr.BuildingBlocks.Membership;
    using Pacr.Infrastructure.Addressing;

    public class LogOffHandler : HttpHandlerBase, IRequiresSessionState
    {
        readonly IMemberService _memberService;

        public LogOffHandler(IMemberService memberService)
            : base()
        {
            _memberService = memberService;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override void ProcessRequestCore(HttpContext context)
        {
            //var httpUser = HttpUser.Restore(context, MemberService);

            //if (httpUser.IsAuthenticated) {
            //    httpUser.LogOff();
            //}

            var map = SiteMapBuilder.Current.GetSiteMapFactory().CreateMap(MemberId.Anonymous);

            context.Response.Redirect(map.Home().AbsoluteUri);
        }
    }
}

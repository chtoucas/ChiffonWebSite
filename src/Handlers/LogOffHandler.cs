namespace Chiffon.Handlers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Services;
    using Narvalo.Web;

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
            //var map = SiteMapBuilder.Current.GetSiteMapFactory().CreateMap(MemberId.Anonymous);

            //context.Response.Redirect(map.Home().AbsoluteUri);
        }
    }
}

namespace Chiffon.Handlers
{
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Services;
    using Narvalo.Web;

    public class LogOffHandler : HttpHandlerBase, IRequiresSessionState
    {
        readonly IMemberService _memberService;
        readonly ISiteMapFactory _siteMapFactory;

        public LogOffHandler(IMemberService memberService, ISiteMapFactory siteMapFactory)
            : base()
        {
            _memberService = memberService;
            _siteMapFactory = siteMapFactory;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override void ProcessRequestCore(HttpContext context)
        {
            FormsAuthentication.SignOut();

            var siteMap = _siteMapFactory.CreateMap(Thread.CurrentThread.CurrentUICulture);
            var nextUrl = siteMap.Home();

            context.Response.Redirect(nextUrl.AbsoluteUri);

            //var map = SiteMapBuilder.Current.GetSiteMapFactory().CreateMap(MemberId.Anonymous);

            //context.Response.Redirect(map.Home().AbsoluteUri);
        }
    }
}

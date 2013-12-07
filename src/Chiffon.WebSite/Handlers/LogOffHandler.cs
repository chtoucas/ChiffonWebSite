namespace Chiffon.Handlers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;
    using Narvalo.Web;

    public class LogOffHandler : HttpHandlerBase, IRequiresSessionState
    {
        //readonly IMemberService _memberService;
        readonly ISiteMapFactory _siteMapFactory;

        public LogOffHandler(
            //IMemberService memberService, 
            ISiteMapFactory siteMapFactory)
            : base()
        {
            //Requires.NotNull(memberService, "memberService");
            Requires.NotNull(siteMapFactory, "siteMapFactory");

            //_memberService = memberService;
            _siteMapFactory = siteMapFactory;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override void ProcessRequestCore(HttpContext context)
        {
            Requires.NotNull(context, "context");

            FormsAuthentication.SignOut();

            var siteMap = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment);
            var nextUrl = siteMap.Home();

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }
    }
}

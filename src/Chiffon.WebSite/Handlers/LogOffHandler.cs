namespace Chiffon.Handlers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Web;

    public class LogOffHandler : HttpHandlerBase, IRequiresSessionState
    {
        //readonly IMemberService _memberService;
        readonly ISiteMap _siteMap;

        public LogOffHandler(
            //IMemberService memberService, 
            ISiteMap siteMap)
            : base()
        {
            //Requires.NotNull(memberService, "memberService");
            Requires.NotNull(siteMap, "siteMap");

            //_memberService = memberService;
            _siteMap = siteMap;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override void ProcessRequestCore(HttpContext context)
        {
            FormsAuthentication.SignOut();

            var nextUrl = _siteMap.Home();

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }
    }
}

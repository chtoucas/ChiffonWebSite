namespace Chiffon.Handlers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Chiffon.Common;
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
            FormsAuthentication.SignOut();

            var nextUrl = SiteMapUtility.GetSiteMap(context).Home();

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }
    }
}

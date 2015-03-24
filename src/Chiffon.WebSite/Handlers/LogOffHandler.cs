namespace Chiffon.Handlers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Common;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Narvalo;
    using Narvalo.Web;

    // TODO: ValidateAntiForgeryToken.
    public class LogOffHandler : HttpHandlerBase, IRequiresSessionState
    {
        readonly ISiteMapFactory _siteMapFactory;

        public LogOffHandler(ISiteMapFactory siteMapFactory)
            : base()
        {
            Require.NotNull(siteMapFactory, "siteMapFactory");

            _siteMapFactory = siteMapFactory;
        }

        public override bool IsReusable { get { return true; } }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override void ProcessRequestCore(HttpContext context)
        {
            //DebugCheck.NotNull(context);

            (new AuthentificationService(context)).SignOut();

            var nextUrl = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment).Home().AbsoluteUri;

            context.Response.Redirect(nextUrl);
        }
    }
}

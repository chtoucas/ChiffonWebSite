namespace Chiffon.Handlers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using Chiffon.Common;
    using Narvalo;
    using Narvalo.Web;

    // TODO: ValidateAntiForgeryToken.
    public sealed class LogOffHandler : HttpHandlerBase, IRequiresSessionState
    {
        private readonly ISiteMapFactory _siteMapFactory;

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
            Check.NotNull(context, "The base class guarantees that the parameter is not null.");

            new AuthenticationService(context).SignOut();

            var nextUrl = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment).Home().AbsoluteUri;

            context.Response.Redirect(nextUrl);
        }
    }
}

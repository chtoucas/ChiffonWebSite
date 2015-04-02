namespace Chiffon.Handlers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using Chiffon.Common;
    using Chiffon.Internal;
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

            IsReusable = true;
        }

        public override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override void ProcessRequestCore(HttpContext context)
        {
            CheckFor.HttpHandlerBase.ProcessRequestCore(context);

            new AuthenticationService(context).SignOut();

            var nextUrl = _siteMapFactory.CreateMap(ChiffonContext.Resolve(HttpContext.Current).Environment).Home().AbsoluteUri;

            context.Response.Redirect(nextUrl);
        }
    }
}

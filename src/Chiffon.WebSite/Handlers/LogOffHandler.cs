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

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override void ProcessRequestCore(HttpContext context)
        {
            Require.NotNull(context, "context");

            (new AuthentificationService(context)).SignOut();

            var siteMap = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment);

            context.Response.Redirect(siteMap.Home().AbsoluteUri);
        }
    }
}

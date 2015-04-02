namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Internal;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Web;

    public sealed class GoHandler
        : HttpHandlerBase<Maybe<Uri>, GoQueryBinder>, IRequiresSessionState
    {
        private readonly ISiteMapFactory _siteMapFactory;

        public GoHandler(ISiteMapFactory siteMapFactory)
            : base()
        {
            Require.NotNull(siteMapFactory, "siteMapFactory");

            _siteMapFactory = siteMapFactory;

            IsReusable = true;
        }

        public override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override void ProcessRequestCore(HttpContext context, Maybe<Uri> query)
        {
            CheckFor.HttpHandlerBase.ProcessRequestCore(context, query);

            var environment = ChiffonContext.Resolve(HttpContext.Current).Environment;
            var siteMap = _siteMapFactory.CreateMap(environment);

            new AuthenticationService(context).SignIn(Member.Anonymous);

            var nextUrl = query.Select(_ => environment.MakeAbsoluteUri(_)).ValueOrElse(siteMap.Home());

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }
    }
}

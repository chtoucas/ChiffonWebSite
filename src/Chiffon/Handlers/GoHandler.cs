namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Internal;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Web;

    // TODO: ValidateAntiForgeryToken.
    public sealed class GoHandler
        : HttpHandlerBase<Maybe<Uri>, GoQueryBinder>, IRequiresSessionState
    {
        private readonly IMemberService _memberService;
        private readonly ISiteMapFactory _siteMapFactory;

        public GoHandler(IMemberService memberService, ISiteMapFactory siteMapFactory)
            : base()
        {
            Require.NotNull(memberService, "memberService");
            Require.NotNull(siteMapFactory, "siteMapFactory");

            _memberService = memberService;
            _siteMapFactory = siteMapFactory;

            IsReusable = true;
        }

        public override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override void ProcessRequestCore(HttpContext context, Maybe<Uri> query)
        {
            CheckFor.HttpHandlerBase.ProcessRequestCore(context, query);

            var environment = ChiffonContext.Resolve(HttpContext.Current).Environment;
            var siteMap = _siteMapFactory.CreateMap(environment);

            var member = new Member {
                Email = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty
            };

            new AuthenticationService(context).SignIn(member);

            var nextUrl = query.Select(_ => environment.MakeAbsoluteUri(_)).ValueOrElse(siteMap.Home());

            context.Response.Redirect(nextUrl.AbsoluteUri);
        }
    }
}

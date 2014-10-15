namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;
    using Narvalo.Web.Security;

    public class GoHandler : HttpHandlerBase<GoQuery>, IRequiresSessionState
    {
        //readonly ChiffonConfig _config;
        readonly IMemberService _memberService;
        readonly IFormsAuthenticationService _formsService;
        readonly ISiteMapFactory _siteMapFactory;

        public GoHandler(
            //ChiffonConfig config,
            IMemberService memberService,
            IFormsAuthenticationService formsService,
            ISiteMapFactory siteMapFactory)
            : base()
        {
            //Requires.NotNull(config, "config");
            Requires.NotNull(memberService, "memberService");
            Requires.NotNull(formsService, "formsService");
            Requires.NotNull(siteMapFactory, "siteMapFactory");

            //_config = config;
            _memberService = memberService;
            _formsService = formsService;
            _siteMapFactory = siteMapFactory;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<GoQuery> Bind(HttpRequest request)
        {
            Requires.NotNull(request, "request");

            var query = request.QueryString;

            var token = query.MayGetValue("token").Filter(_ => _.Length > 0);
            if (token.IsNone) { return BindingFailure("token"); }

            var model = new GoQuery { Token = token.Value };

            return Outcome<GoQuery>.Success(model);
        }

        protected override void ProcessRequestCore(HttpContext context, GoQuery query)
        {
            Requires.NotNull(context, "context");
            Requires.NotNull(query, "query");

            var userName = _memberService.LogOn(query.Token);

            var succeed = !String.IsNullOrEmpty(userName);
            if (succeed) {
                _formsService.SignIn(userName, false /* createPersistentCookie */);
            }

            var siteMap = _siteMapFactory.CreateMap(ChiffonContext.Current.Environment);

            Uri nextUrl = succeed ? siteMap.Home() : siteMap.LogOn();

            context.Response.Redirect(nextUrl.ToString());
        }
    }
}

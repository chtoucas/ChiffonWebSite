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
        readonly ISiteMap _siteMap;

        public GoHandler(
            //ChiffonConfig config,
            IMemberService memberService,
            IFormsAuthenticationService formsService,
            ISiteMap siteMap)
            : base()
        {
            //Requires.NotNull(config, "config");
            Requires.NotNull(memberService, "memberService");
            Requires.NotNull(formsService, "formsService");
            Requires.NotNull(siteMap, "siteMap");

            //_config = config;
            _memberService = memberService;
            _formsService = formsService;
            _siteMap = siteMap;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<GoQuery> Bind(HttpRequest request)
        {
            var query = request.QueryString;

            var token = query.MayGetValue("token").Filter(_ => _.Length > 0);
            if (token.IsNone) { return BindingFailure("token"); }

            var model = new GoQuery { Token = token.Value };

            return Outcome<GoQuery>.Success(model);
        }

        protected override void ProcessRequestCore(HttpContext context, GoQuery query)
        {
            var userName = _memberService.LogOn(query.Token);

            var succeed = !String.IsNullOrEmpty(userName);
            if (succeed) {
                _formsService.SignIn(userName, false /* createPersistentCookie */);
            }

            Uri nextUrl = succeed ? _siteMap.Home() : _siteMap.LogOn();

            context.Response.Redirect(nextUrl.ToString());
        }
    }
}

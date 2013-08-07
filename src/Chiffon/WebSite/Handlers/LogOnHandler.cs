namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class LogOnHandler : HttpHandlerBase<LogOnQuery>, IRequiresSessionState
    {
        readonly IMemberService _memberService;

        public LogOnHandler(IMemberService memberService)
            : base()
        {
            _memberService = memberService;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            var form = request.Form;

            var token = form.MayGetValue("token");
            if (token.IsNone) { return BindingFailure("token"); }

            var targetUrl = form.MayParseValue("targeturl", _ => MayParse.ToUri(_, UriKind.Relative));

            var model = new LogOnQuery { TargetUrl = targetUrl, Token = token.Value };

            return Outcome<LogOnQuery>.Success(model);
        }

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            //// TODO: loguer context.Request.UserHostAddress.
            bool succeed = _memberService.LogOn(query.Token);

            //var map = SiteMapBuilder.Current.GetSiteMapFactory().CreateMap(memberId);

            //Uri nextUrl
            //    = memberId.IsAnonymous
            //    ? map.LogOn(query)
            //    : query.TargetUrl != null
            //        ? map.MakeAbsoluteUri(query.TargetUrl)
            //        : map.Home();

            //context.Response.Redirect(nextUrl.AbsoluteUri);
        }
    }
}

namespace Chiffon.Handlers
{
    using System;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Services;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class LogOnHandler : HttpHandlerBase<LogOnQuery>, IRequiresSessionState
    {
        readonly IMemberService _memberService;
        readonly ISiteMapFactory _siteMapFactory;

        public LogOnHandler(IMemberService memberService, ISiteMapFactory siteMapFactory)
            : base()
        {
            _memberService = memberService;
            _siteMapFactory = siteMapFactory;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Post; } }

        protected override Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            var form = request.Form;

            var token = form.MayGetValue("token").Filter(_ => _.Length > 0);
            if (token.IsNone) { return BindingFailure("token"); }

            var targetUrl = form.MayGetValue("targeturl").Filter(_ => _.Length > 0)
                .Bind(_ => MayParse.ToUri(_, UriKind.Relative));

            var model = new LogOnQuery { TargetUrl = targetUrl, Token = token.Value };

            return Outcome<LogOnQuery>.Success(model);
        }

        protected override void ProcessRequestCore(HttpContext context, LogOnQuery query)
        {
            var succeed = _memberService.LogOn(query.Token, true /* createPersistentCookie */);
            var siteMap = _siteMapFactory.CreateMap(Thread.CurrentThread.CurrentUICulture);

            // TODO: Si TargetUrl a une valeur, alors on redirige vers une URL relative.
            Uri nextUrl = succeed
                ? siteMap.Home() // query.TargetUrl.ValueOrElse(siteMap.Home())
                : query.TargetUrl.Match(_ => siteMap.LogOn(_), siteMap.LogOn());

            context.Response.Write(nextUrl.ToString());
            //context.Response.Redirect(nextUrl.ToString());

            //var map = SiteMapBuilder.Current.GetSiteMapFactory().CreateMap(memberId);
        }
    }
}

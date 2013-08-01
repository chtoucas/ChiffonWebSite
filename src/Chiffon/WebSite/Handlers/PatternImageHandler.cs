namespace Chiffon.WebSite.Handlers
{
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Crosscuttings;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class PatternImageHandler : HttpHandlerBase<PatternImage>, IRequiresSessionState
    {
        const int MinutesInCache_ = 30;

        readonly ChiffonConfig _config;

        public PatternImageHandler(ChiffonConfig config)
            : base()
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<PatternImage> Bind(HttpRequest request)
        {
            var id = request.QueryString.MayParseValue("id", _ => MayParse.ToInt32(_));
            if (id.IsNone) { return Outcome<PatternImage>.Failure(Error.Create("Id")); }

            var query = new PatternImage { Id = id.Value };

            return Outcome<PatternImage>.Success(query);
        }

        protected override void ProcessRequestCore(HttpContext context, PatternImage query)
        {
            string path = Path.Combine(_config.PatternDirectory, @"viviane-devaux\motif5.jpg");

            context.Response.Clear();
            context.Response.PrivatelyCacheFor(0, 0, MinutesInCache_);
            context.Response.ContentType = "image/jpeg";
            context.Response.TransmitFile(path);
        }
    }
}

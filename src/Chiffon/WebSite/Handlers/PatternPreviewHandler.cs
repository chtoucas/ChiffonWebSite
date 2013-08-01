namespace Chiffon.WebSite.Handlers
{
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Crosscuttings;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class PatternPreviewHandler : HttpHandlerBase<PatternPreviewQuery>, IRequiresSessionState
    {
        const int MinutesInCache_ = 30;

        readonly ChiffonConfig _config;

        public PatternPreviewHandler(ChiffonConfig config)
            : base()
        {
            Requires.NotNull(config, "config");

            _config = config;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<PatternPreviewQuery> Bind(HttpRequest request)
        {
            return new PatternPreviewQueryBinder().Bind(request);
        }

        protected override void ProcessRequestCore(HttpContext context, PatternPreviewQuery query)
        {
            string path = Path.Combine(_config.PatternDirectory, @"viviane-devaux\motif1_apercu.jpg");

            context.Response.Clear();
            context.Response.PrivatelyCacheFor(0, 0, MinutesInCache_);
            context.Response.ContentType = "image/jpeg";
            context.Response.TransmitFile(path);
        }
    }
}

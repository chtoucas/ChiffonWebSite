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

    public class PatternPreviewHandler : HttpHandlerBase<PatternPreview>, IRequiresSessionState
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

        protected override Outcome<PatternPreview> Bind(HttpRequest request)
        {
            var nvc = request.QueryString;

            // > Paramètres obligatoires <

            var id = nvc.MayParseValue("id", _ => MayParse.ToInt32(_));
            if (id.IsNone) {
                return Outcome<PatternPreview>.Failure("XXX");
            }

            var width = nvc.MayParseValue("width", _ => MayParse.ToInt32(_));
            if (width.IsNone) {
                return Outcome<PatternPreview>.Failure("XXX");
            }

            var height = nvc.MayParseValue("height", _ => MayParse.ToInt32(_));
            if (height.IsNone) {
                return Outcome<PatternPreview>.Failure("XXX");
            }

            // > Création du modèle <

            var query = new PatternPreview {
                Height = height.Value,
                Id = id.Value,
                Width = width.Value,
            };

            return Outcome<PatternPreview>.Success(query);
        }

        protected override void ProcessRequestCore(HttpContext context, PatternPreview query)
        {
            string path = Path.Combine(_config.PatternDirectory, @"viviane-devaux\motif1_apercu.jpg");

            context.Response.Clear();
            context.Response.PrivatelyCacheFor(0, 0, MinutesInCache_);
            context.Response.ContentType = "image/jpeg";
            context.Response.TransmitFile(path);
        }
    }
}

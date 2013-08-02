namespace Chiffon.WebSite.Handlers
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Application;
    using Chiffon.Crosscuttings;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class PatternPreviewHandler : HttpHandlerBase<PatternPreview>, IRequiresSessionState
    {
        readonly ChiffonConfig _config;
        readonly IPatternService _service;

        public PatternPreviewHandler(ChiffonConfig config, IPatternService service)
            : base()
        {
            Requires.NotNull(config, "config");
            Requires.NotNull(service, "service");

            _config = config;
            _service = service;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<PatternPreview> Bind(HttpRequest request)
        {
            var nvc = request.QueryString;

            // > Paramètres obligatoires <

            var height = nvc.MayParseValue("height", _ => MayParse.ToInt32(_));
            if (height.IsNone) {
                return Outcome<PatternPreview>.Failure("XXX");
            }

            var id = nvc.MayGetValue("id");
            if (id.IsNone) {
                return Outcome<PatternPreview>.Failure("XXX");
            }

            var memberKey = nvc.MayGetValue("member");
            if (memberKey.IsNone) {
                return Outcome<PatternPreview>.Failure("XXX");
            }

            var width = nvc.MayParseValue("width", _ => MayParse.ToInt32(_));
            if (width.IsNone) {
                return Outcome<PatternPreview>.Failure("XXX");
            }

            // > Création du modèle <

            var query = new PatternPreview {
                Height = height.Value,
                Id = id.Value,
                MemberKey = memberKey.Value,
                Width = width.Value,
            };

            return Outcome<PatternPreview>.Success(query);
        }

        protected override void ProcessRequestCore(HttpContext context, PatternPreview query)
        {
            var result = _service.FindPattern(query.Id, query.MemberKey);
            if (result.IsNone) {
                context.Response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }
            var dto = result.Value;
            var pattern = dto.Pattern;
            var member = dto.Member;

            var fileSystem = new PatternFileSystem(_config);
            var filePath = fileSystem.GetPath(pattern, member, new PatternSize(query.Width, query.Height));

            if (filePath.IsNone) {
                context.Response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            context.Response.Clear();
            // On instruit le client de cacher le motif.
            if (pattern.IsPublic) {
                context.Response.PubliclyCacheFor(1, 0, 0);
            }
            else {
                context.Response.PrivatelyCacheFor(0, 0, 30);
            }
            context.Response.ContentType = PatternFileSystem.MimeType;
            context.Response.TransmitFile(filePath.Value);
        }
    }
}

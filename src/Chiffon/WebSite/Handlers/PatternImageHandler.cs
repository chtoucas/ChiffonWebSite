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

    public class PatternImageHandler : HttpHandlerBase<PatternImage>, IRequiresSessionState
    {
        const int MinutesInCache_ = 30;

        readonly ChiffonConfig _config;
        readonly IPatternService _service;

        public PatternImageHandler(ChiffonConfig config, IPatternService service)
            : base()
        {
            Requires.NotNull(config, "config");
            Requires.NotNull(service, "service");

            _config = config;
            _service = service;
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<PatternImage> Bind(HttpRequest request)
        {
            var nvc = request.QueryString;

            var designerKey = nvc.MayGetValue("designer");
            if (designerKey.IsNone) { return MissingOrInvalidParameterOutcome("designer"); }

            var reference = nvc.MayGetValue("ref");
            if (reference.IsNone) { return MissingOrInvalidParameterOutcome("ref"); }

            var query = new PatternImage {
                DesignerUrlKey = designerKey.Value,
                Reference = reference.Value
            };

            return Outcome<PatternImage>.Success(query);
        }

        protected override void ProcessRequestCore(HttpContext context, PatternImage query)
        {
            var result = _service.FindPatternFile(query.Reference, query.DesignerUrlKey);
            if (result.IsNone) {
                context.Response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            var file = result.Value;

            var fileSystem = new PatternFileSystem(_config);
            var filePath = fileSystem.GetPath(file);

            context.Response.Clear();
            if (file.IsPublic) {
                context.Response.PubliclyCacheFor(1, 0, 0);
            }
            else {
                context.Response.PrivatelyCacheFor(0, 0, 30);
            }
            context.Response.ContentType = PatternFile.MimeType;
            context.Response.TransmitFile(filePath);
        }
    }
}

namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Services;
    using Chiffon.Crosscuttings;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class PatternImageHandler : HttpHandlerBase<PatternImageQuery>, IRequiresSessionState
    {
        const int MinutesInCache_ = 30;

        // Mise en cache pour une journée.
        static readonly TimeSpan PublicCacheTimeSpan_ = new TimeSpan(1, 0, 0, 0);
        // Mise en cache pour 30 minutes.
        static readonly TimeSpan PrivateCacheTimeSpan_ = new TimeSpan(1, 0, 0);

        readonly ChiffonConfig _config;
        readonly PatternFileSystem _fileSystem;
        readonly IPatternService _service;

        public PatternImageHandler(ChiffonConfig config, IPatternService service)
            : base()
        {
            Requires.NotNull(config, "config");
            Requires.NotNull(service, "service");

            _config = config;
            _service = service;

            _fileSystem = new PatternFileSystem(_config);
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<PatternImageQuery> Bind(HttpRequest request)
        {
            var nvc = request.QueryString;

            var designerUrlKey = nvc.MayGetValue("designer");
            if (designerUrlKey.IsNone) { return MissingOrInvalidParameterOutcome("designer"); }

            var size = nvc.MayParseValue("size", _ => MayParse.ToEnum<PatternSize>(_));
            if (size.IsNone) { return MissingOrInvalidParameterOutcome("size"); }

            var reference = nvc.MayGetValue("ref");
            if (reference.IsNone) { return MissingOrInvalidParameterOutcome("ref"); }

            var query = new PatternImageQuery {
                DesignerUrlKey = designerUrlKey.Value,
                Reference = reference.Value,
                Size = size.Value,
            };

            return Outcome<PatternImageQuery>.Success(query);
        }

        protected override void ProcessRequestCore(HttpContext context, PatternImageQuery query)
        {
            var response = context.Response;

            // FIXME: Ajouter le filtre "publique ou non".
            var result_ = _service.MayFindPatternFile(query.Reference, query.DesignerUrlKey, true /* publicOnly */);
            if (result_.IsNone) {
                response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            var result = result_.Value;
            var filePath = _fileSystem.GetPath(PatternFile.Create(result.Directory, result.Reference, query.Size));

            response.Clear();
            if (result.IsPublic) {
                response.PubliclyCacheFor(PublicCacheTimeSpan_);
            }
            else {
                response.PrivatelyCacheFor(PrivateCacheTimeSpan_);
            }
            response.ContentType = PatternFile.MimeType;
            response.TransmitFile(filePath);
        }
    }
}

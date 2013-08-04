namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Crosscuttings;
    using Chiffon.Infrastructure;
    using Chiffon.Persistence;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class PatternImageHandler : HttpHandlerBase<PatternImageQuery>, IRequiresSessionState
    {
        // Mise en cache pour une journée.
        static readonly TimeSpan PublicCacheTimeSpan_ = new TimeSpan(1, 0, 0, 0);
        // Mise en cache pour 30 minutes.
        static readonly TimeSpan PrivateCacheTimeSpan_ = new TimeSpan(1, 0, 0);

        readonly ChiffonConfig _config;
        readonly PatternFileSystem _fileSystem;
        readonly IQueryService _service;

        public PatternImageHandler(ChiffonConfig config, IQueryService service)
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

            var designerKey = nvc.MayGetValue("designer").Filter(_ => _.Length > 1);
            if (designerKey.IsNone) { return BindingFailure("designer"); }

            var size = nvc.MayParseValue("size", _ => MayParse.ToEnum<PatternSize>(_));
            if (size.IsNone) { return BindingFailure("size"); }

            var reference = nvc.MayGetValue("ref").Filter(_ => _.Length > 1);
            if (reference.IsNone) { return BindingFailure("ref"); }

            var query = new PatternImageQuery {
                DesignerKey = designerKey.Value,
                Reference = reference.Value,
                Size = size.Value,
            };

            return Outcome<PatternImageQuery>.Success(query);
        }

        protected override void ProcessRequestCore(HttpContext context, PatternImageQuery query)
        {
            var response = context.Response;

            // FIXME: Ajouter le filtre "publique ou non".
            var result_ = _service.MayGetPattern(query.Reference, query.DesignerKey, true /* publicOnly */);
            if (result_.IsNone) {
                response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            var result = result_.Value;
            var imagePath = _fileSystem.GetPath(
                PatternImage.Create(result.Directory, result.Reference, query.Size));

            response.Clear();
            if (result.IsPublic) {
                response.PubliclyCacheFor(PublicCacheTimeSpan_);
            }
            else {
                response.PrivatelyCacheFor(PrivateCacheTimeSpan_);
            }
            response.ContentType = PatternImage.MimeType;
            response.TransmitFile(imagePath);
        }
    }
}

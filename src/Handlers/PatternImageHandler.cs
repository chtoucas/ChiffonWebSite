namespace Chiffon.Handlers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Data;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class PatternImageHandler : HttpHandlerBase<PatternImageQuery>, IRequiresSessionState
    {
        static readonly TimeSpan PublicCacheTimeSpan_ = new TimeSpan(7, 0, 0, 0);
        static readonly TimeSpan PrivateCacheTimeSpan_ = new TimeSpan(1, 0, 0, 0);

        readonly PatternFileSystem _fileSystem;
        readonly IQueries _queries;

        public PatternImageHandler(ChiffonConfig config, IQueries queries)
            : base()
        {
            Requires.NotNull(config, "config");
            Requires.NotNull(queries, "queries");

            _queries = queries;

            _fileSystem = new PatternFileSystem(config);
        }

        // TODO: Pour le moment il n'est pas opportun de réutiliser cet Handler car IQueries 
        // peut avoir des dépendances vis à vis de la requête en cours.
        public override bool IsReusable { get { return false; } }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<PatternImageQuery> Bind(HttpRequest request)
        {
            var nvc = request.QueryString;

            var designerKey = nvc.MayParseValue("designer", _ => DesignerKey.MayParse(_));
            if (designerKey.IsNone) { return BindingFailure("designer"); }

            var size = nvc.MayParseValue("size", _ => MayParse.ToEnum<PatternSize>(_));
            if (size.IsNone) { return BindingFailure("size"); }

            var reference = nvc.MayGetValue("reference").Filter(_ => _.Length > 0);
            if (reference.IsNone) { return BindingFailure("reference"); }

            var version = nvc.MayGetValue("version");
            if (version.IsNone) { return BindingFailure("reference"); }

            var query = new PatternImageQuery {
                DesignerKey = designerKey.Value,
                Reference = reference.Value,
                Size = size.Value,
                Version = version.Value,
            };

            return Outcome<PatternImageQuery>.Success(query);
        }

        protected override void ProcessRequestCore(HttpContext context, PatternImageQuery query)
        {
            var response = context.Response;

            // FIXME
            var pattern = _queries.GetPattern(query.DesignerKey, query.Reference, query.Version);
            if (pattern == null) {
                response.SetStatusCode(HttpStatusCode.NotFound); return;
            }

            var visibility = pattern.GetVisibility(query.Size);

            bool isAuth = context.User.Identity.IsAuthenticated;

            switch (visibility) {
                case PatternVisibility.Members:
                    if (!isAuth) { response.SetStatusCode(HttpStatusCode.Unauthorized); return; }
                    break;
                case PatternVisibility.Public:
                    break;
                case PatternVisibility.None:
                default:
                    response.SetStatusCode(HttpStatusCode.NotFound); return;
            }

            var image = pattern.GetImage(query.Size);

            // TODO: Il faut revoir les en-têtes de cache.
            response.Clear();
            if (visibility == PatternVisibility.Public) {
                response.PubliclyCacheFor(PublicCacheTimeSpan_);
            }
            else {
                response.PrivatelyCacheFor(PrivateCacheTimeSpan_);
            }
            response.ContentType = image.MimeType;
            response.TransmitFile(_fileSystem.GetPath(image));
        }
    }
}

namespace Chiffon.Handlers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.Persistence;
    using Narvalo;
    using Narvalo.Web;

    public class PatternImageHandler
        : HttpHandlerBase<PatternImageQuery, PatternImageQueryBinder>, IRequiresSessionState
    {
        // Mise en cache publique pour 7 jours.
        static readonly TimeSpan PublicCacheTimeSpan_ = new TimeSpan(7, 0, 0, 0);
        // Mise en cache publique pour 1 jour.
        static readonly TimeSpan PrivateCacheTimeSpan_ = new TimeSpan(1, 0, 0, 0);

        readonly ChiffonConfig _config;
        readonly PatternFileSystem _fileSystem;
        readonly IDbQueries _queries;

        public PatternImageHandler(ChiffonConfig config, IDbQueries queries)
            : base()
        {
            Require.NotNull(config, "config");
            Require.NotNull(queries, "queries");

            _config = config;
            _queries = queries;

            _fileSystem = new PatternFileSystem(config);
        }

        // TODO: Pour le moment il n'est pas opportun de réutiliser cet Handler car IDbQueries 
        // peut avoir des dépendances vis à vis de la requête en cours.
        //public override bool IsReusable { get { return true; } }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override void ProcessRequestCore(HttpContext context, PatternImageQuery query)
        {
            DebugCheck.NotNull(context);
            DebugCheck.NotNull(query);

            var response = context.Response;

            var pattern = _queries.GetPattern(query.DesignerKey, query.Reference, query.Variant);
            if (pattern == null) {
                response.SetStatusCode(HttpStatusCode.NotFound); return;
            }

            var size = query.Size;

            // FIXME
            if (size == PatternSize.Preview && !pattern.HasPreview) {
                response.SetStatusCode(HttpStatusCode.NotFound); return;
            }

            var visibility = pattern.GetVisibility(size);

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

            var image = pattern.GetImage(size);

            // FIXME: Capturer les exceptions d'IO.
            response.Clear();
            if (_config.EnableClientCache) {
                CacheResponse_(response, visibility);
            }
            response.ContentType = image.MimeType;
            response.TransmitFile(_fileSystem.GetPath(image));
        }

        // TODO: Il faut revoir les en-têtes de cache.
        static void CacheResponse_(HttpResponse response, PatternVisibility visibility)
        {
            if (visibility == PatternVisibility.Public) {
                response.PubliclyCacheFor(PublicCacheTimeSpan_);
            }
            else {
                response.PrivatelyCacheFor(PrivateCacheTimeSpan_);
            }
        }
    }
}

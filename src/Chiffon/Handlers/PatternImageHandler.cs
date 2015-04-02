namespace Chiffon.Handlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Internal;
    using Chiffon.Persistence;
    using Narvalo;
    using Narvalo.Web;

    public sealed class PatternImageHandler
        : HttpHandlerBase<PatternImageQuery, PatternImageQueryBinder>, IRequiresSessionState
    {
        // Mise en cache publique pour 7 jours.
        private static readonly TimeSpan s_PublicCacheTimeSpan = new TimeSpan(7, 0, 0, 0);
        // Mise en cache publique pour 1 jour.
        private static readonly TimeSpan s_PrivateCacheTimeSpan = new TimeSpan(1, 0, 0, 0);

        private readonly ChiffonConfig _config;
        private readonly IDbQueries _queries;

        public PatternImageHandler(ChiffonConfig config, IDbQueries queries)
            : base()
        {
            Require.NotNull(config, "config");
            Require.NotNull(queries, "queries");

            _config = config;
            _queries = queries;
        }

        // TODO: Pour le moment il n'est pas opportun de réutiliser ce gestionnaire car IDbQueries 
        // peut avoir des dépendances vis à vis de la requête en cours.
        //public override bool IsReusable { get { return true; } }

        public override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override void ProcessRequestCore(HttpContext context, PatternImageQuery query)
        {
            CheckFor.HttpHandlerBase.ProcessRequestCore(context, query);

            var response = context.Response;

            var pattern = _queries.GetPattern(query.DesignerKey, query.Reference, query.Variant);
            if (pattern == null)
            {
                response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }

#if SHOWCASE // Seules les images publiques et celles de Vivi sont visibles.
            if (!pattern.Showcased && query.DesignerKey != DesignerKey.VivianeDevaux)
            {
                // Renvoyer une réponse no-content aurait été plus simple
                // mais cela donne une icone cassée dans Chrome.
                response.SetStatusCode(HttpStatusCode.OK);
                response.PrivatelyCacheFor(s_PrivateCacheTimeSpan);
                response.TransmitFile("~/1x1.png");
                return;
            }
#endif

            var size = query.Size;

            // FIXME
            if (size == PatternSize.Preview && !pattern.HasPreview)
            {
                response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            var visibility = pattern.GetVisibility(size);

            bool isAuth = context.User.Identity.IsAuthenticated;

            switch (visibility)
            {
                case PatternVisibility.Members:
                    if (!isAuth)
                    {
                        response.SetStatusCode(HttpStatusCode.Unauthorized);
                        return;
                    }

                    break;
                case PatternVisibility.Public:
                    break;
                case PatternVisibility.None:
                default:
                    response.SetStatusCode(HttpStatusCode.NotFound); 
                    return;
            }

            var image = pattern.GetImage(size);

            // FIXME: Capturer les exceptions d'IO.
            response.Clear();
            if (_config.EnableClientCache)
            {
                CacheResponse_(response, visibility);
            }

            response.ContentType = image.MimeType;
            response.TransmitFile(GetPath_(image));
        }

        // TODO: Il faut revoir les en-têtes de cache.
        private static void CacheResponse_(HttpResponse response, PatternVisibility visibility)
        {
            Contract.Requires(response != null);

            if (visibility == PatternVisibility.Public)
            {
                response.PubliclyCacheFor(s_PublicCacheTimeSpan);
            }
            else
            {
                response.PrivatelyCacheFor(s_PrivateCacheTimeSpan);
            }
        }

        private string GetPath_(PatternImage image)
        {
            Require.NotNull(image, "image");
            Contract.Ensures(Contract.Result<string>() != null);

            return Path.Combine(_config.PatternDirectory, image.RelativePath);
        }
    }
}

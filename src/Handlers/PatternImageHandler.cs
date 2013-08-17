namespace Chiffon.Handlers
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using System.Web.Caching;
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
        const int CacheExpirationInMinutes_ = 30;

        static object Lock_ = new Object();
        // Mise en cache pour une journée.
        static readonly TimeSpan PublicCacheTimeSpan_ = new TimeSpan(365, 0, 0, 0);
        // Mise en cache pour 30 minutes.
        static readonly TimeSpan PrivateCacheTimeSpan_ = new TimeSpan(1, 0, 0);

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

            var result = GetImage_(context, query.DesignerKey, query.Reference, query.Size);
            if (result == null) {
                response.SetStatusCode(HttpStatusCode.NotFound); return;
            }

            PatternVisibility visibility = result.Item1;
            PatternImage image = result.Item2;

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

            // TODO: http://markusgreuel.net/blog/website-performance-with-asp-net-part4-use-cache-headers
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

        string GetCacheKey_(DesignerKey designerKey, string reference)
        {
            return String.Format(CultureInfo.InvariantCulture, "image_{0}_{1}", designerKey.ToString(), reference);
        }

        Tuple<PatternVisibility, PatternImage> GetImage_(
            HttpContext context, DesignerKey designerKey, string reference, PatternSize size)
        {
            Pattern pattern;

            var cache = context.Cache;
            var cacheKey = GetCacheKey_(designerKey, reference);
            var cacheValue = cache[cacheKey] as Pattern;

            if (cacheValue == null) {
                pattern = _queries.GetPattern(designerKey, reference);

                if (pattern == null) { return null; }

                lock (Lock_) {
                    if (cache[cacheKey] == null) {
                        cache.Add(cacheKey, pattern, null,
                            DateTime.Now.AddMinutes(CacheExpirationInMinutes_),
                            Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    }
                }
            }
            else {
                pattern = cacheValue;
            }

            return Tuple.Create(pattern.GetVisibility(size), pattern.GetImage(size));
        }
    }
}

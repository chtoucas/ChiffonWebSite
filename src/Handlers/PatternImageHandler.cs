namespace Chiffon.Handlers
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Crosscuttings;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Data;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class PatternImageHandler : HttpHandlerBase<PatternImageQuery>, IRequiresSessionState
    {
        const int CacheExpirationInMinutes_ = 30;

        static object Lock_ = new Object();
        // Mise en cache pour une journée.
        static readonly TimeSpan PublicCacheTimeSpan_ = new TimeSpan(1, 0, 0, 0);
        // Mise en cache pour 30 minutes.
        static readonly TimeSpan PrivateCacheTimeSpan_ = new TimeSpan(1, 0, 0);

        readonly PatternFileSystem _fileSystem;
        readonly DbHelper _dbHelper;

        public PatternImageHandler(ChiffonConfig config, DbHelper dbHelper)
            : base()
        {
            Requires.NotNull(config, "config");
            Requires.NotNull(dbHelper, "dbHelper");

            _dbHelper = dbHelper;

            _fileSystem = new PatternFileSystem(config);
        }

        protected override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        protected override Outcome<PatternImageQuery> Bind(HttpRequest request)
        {
            var nvc = request.QueryString;

            var designerKey = nvc.MayParseValue("designer", _ => DesignerKey.Parse(_));
            if (designerKey.IsNone) { return BindingFailure("designer"); }

            var size = nvc.MayParseValue("size", _ => MayParse.ToEnum<PatternSize>(_));
            if (size.IsNone) { return BindingFailure("size"); }

            var reference = nvc.MayGetValue("reference").Filter(_ => _.Length > 1);
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

            var result_ = MayGetImage_(context, query.DesignerKey, query.Reference, query.Size);
            if (result_.IsNone) {
                response.SetStatusCode(HttpStatusCode.NotFound); return;
            }

            var result = result_.Value;
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

        Maybe<Tuple<PatternVisibility, PatternImage>> MayGetImage_(
            HttpContext context, DesignerKey designerKey, string reference, PatternSize size)
        {
            var pattern = Maybe<Pattern>.None;

            var cache = context.Cache;
            var cacheKey = GetCacheKey_(designerKey, reference);
            var cacheValue = cache[cacheKey] as Pattern;

            if (cacheValue == null) {
                pattern = LoadPattern_(designerKey, reference);

                if (pattern.IsSome) {
                    lock (Lock_) {
                        if (cache[cacheKey] == null) {
                            cache.Add(cacheKey, pattern.Value, null, 
                                DateTime.Now.AddMinutes(CacheExpirationInMinutes_),
                                Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                        }
                    }
                }
            }
            else {
                pattern = Maybe.Create(cacheValue);
            }

            return pattern.Map(_ => Tuple.Create(_.GetVisibility(size), _.GetImage(size)));
        }

        Maybe<Pattern> LoadPattern_(DesignerKey designerKey, string reference)
        {
            var result = Maybe<Pattern>.None;

            using (var cnx = _dbHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_getPattern";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameterCollection p = cmd.Parameters;
                    p.Add("@reference", SqlDbType.NVarChar).Value = reference;
                    p.Add("@designer", SqlDbType.NVarChar).Value = designerKey.Key;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (rdr.Read()) {
                            var pattern = new Pattern(new PatternId(designerKey, reference)) {
                                //CreationTime = rdr.GetDateTimeColumn("creation_time"),
                                Preferred = rdr.GetBooleanColumn("preferred"),
                                Published = rdr.GetBooleanColumn("online"),
                                Showcased = rdr.GetBooleanColumn("showcased"),
                            };

                            result = Maybe.Create(pattern);
                        }
                    }
                }
            }

            return result;
        }
    }
}

﻿namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Chiffon.Crosscuttings;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.Services;
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

            // FIXME:
            bool isAuth = true;

            var result_ = _service.MayGetPattern(new PatternId(query.DesignerKey, query.Reference));
            if (result_.IsNone) {
                response.SetStatusCode(HttpStatusCode.NotFound);
                return;
            }

            var pattern = result_.Value;
            var visibility = pattern.GetVisibility(query.Size);
            var isPublic = visibility == PatternVisibility.Public;
            var isVisible = isPublic || (isAuth && visibility == PatternVisibility.Members);

            if (!isVisible) {
                response.SetStatusCode(HttpStatusCode.Unauthorized);
                return;
            }

            var image = pattern.GetImage(query.Size);
            var imagePath = _fileSystem.GetPath(image);

            response.Clear();
            if (isPublic) {
                response.PubliclyCacheFor(PublicCacheTimeSpan_);
            }
            else {
                response.PrivatelyCacheFor(PrivateCacheTimeSpan_);
            }
            response.ContentType = image.MimeType;
            response.TransmitFile(imagePath);
        }
    }
}

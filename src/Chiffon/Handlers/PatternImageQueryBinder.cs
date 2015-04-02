﻿namespace Chiffon.Handlers
{
    using System;
    using System.Web;

    using Chiffon.Common;
    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class PatternImageQueryBinder : HttpQueryBinderBase<PatternImageQuery>
    {
        public PatternImageQueryBinder() : base() { }

        protected override Maybe<PatternImageQuery> BindCore(HttpRequest request)
        {
            var nvc = request.QueryString;

            return
                from designerKey in
                    Maybe.Flatten(nvc.MayGetSingle("designerKey").Select(_ => DesignerKey.MayParse(_)))
                from reference in nvc.MayGetSingle("reference")
                from size in
                    (from _ in nvc.MayGetSingle("size") select ParseTo.Enum<PatternImageSize>(_))
                let variant = nvc.MayGetSingle("variant")
                where size.HasValue
                select new PatternImageQuery {
                    DesignerKey = designerKey,
                    Reference = reference,
                    Size = size.Value,
                    Variant = variant.ValueOrElse(String.Empty),
                };
        }
    }
}
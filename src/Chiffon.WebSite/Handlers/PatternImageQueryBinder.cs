namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Linq;
    using Narvalo.Web;

    public class PatternImageQueryBinder : HttpQueryBinderBase<PatternImageQuery>
    {
        public PatternImageQueryBinder() : base() { }

        protected override Maybe<PatternImageQuery> BindCore(HttpRequest request)
        {
            var nvc = request.QueryString;

            return
                // Paramètres obligatoires.
                from designerKey in
                    (from _ in nvc.MayGetSingle("designerKey") select DesignerKey.MayParse(_))
                from reference in nvc.MayGetSingle("reference")
                from size in
                    (from _ in nvc.MayGetSingle("size") select ParseTo.NullableEnum<PatternSize>(_))

                // Paramètres optionnelles.
                let variant = nvc.MayGetSingle("variant")

                where designerKey.IsSome && size.HasValue

                select new PatternImageQuery {
                    DesignerKey = designerKey.Value,
                    Reference = reference,
                    Size = size.Value,
                    Variant = variant.ValueOrElse(String.Empty),
                };
        }
    }
}
namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Linq;
    using Narvalo.Web;

    public class PatternImageQueryBinder : IQueryBinder<PatternImageQuery>
    {
        Maybe<DesignerKey> DesignerKey { get; set; }
        Maybe<string> Reference { get; set; }
        Maybe<PatternSize> Size { get; set; }
        Maybe<string> Variant { get; set; }

        public bool CanValidate { get; private set; }

        public PatternImageQuery Bind(HttpRequest request)
        {
            Require.NotNull(request, "request");

            var result = new PatternImageQuery();

            var nvc = request.QueryString;

            DesignerKey = nvc.MayParseValue("designer", _ => Chiffon.Entities.DesignerKey.MayParse(_))
                .OnSome(_ => result.DesignerKey = _);

            Reference = nvc.MayGetValue("reference")
                .OnSome(_ => result.Reference = _);

            Size = nvc.MayParseValue("size", _ => MayParse.ToEnum<PatternSize>(_))
                .OnSome(_ => result.Size = _);

            Variant = nvc.MayGetValue("version");

            CanValidate = true;

            return result;
        }

        public bool Validate()
        {
            if (!CanValidate) {
                throw new InvalidOperationException("FIXME");
            }

            return DesignerKey.IsSome
                && Reference.Filter(_ => _.Length > 0).IsSome
                && Size.IsSome
                && Variant.IsSome;
        }
    }
}
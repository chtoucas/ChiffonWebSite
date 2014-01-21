namespace Chiffon.Handlers
{
    using System.Web;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Linq;
    using Narvalo.Web;

    public class PatternImageQueryBinder : QueryBinderBase<PatternImageQuery>
    {
        public PatternImageQueryBinder() : base() { }

        protected override PatternImageQuery BindCore(HttpRequest request)
        {
            var result = new PatternImageQuery();

            var nvc = request.QueryString;

            nvc.MayParseValue("designerkey", _ => DesignerKey.MayParse(_))
                .OnSome(_ => result.DesignerKey = _);

            nvc.MayGetValue("reference")
                .OnSome(_ => result.Reference = _);

            nvc.MayParseValue("size", _ => MayParse.ToEnum<PatternSize>(_))
                .OnSome(_ => result.Size = _);

            nvc.MayGetValue("variant")
                .OnSome(_ => result.Variant = _);

            return result;
        }
    }
}
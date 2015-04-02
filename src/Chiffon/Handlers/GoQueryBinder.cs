namespace Chiffon.Handlers
{
    using System;
    using System.Web;

    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class GoQueryBinder : HttpQueryBinderBase<Maybe<Uri>>
    {
        public GoQueryBinder() : base() { }

        protected override Maybe<Maybe<Uri>> BindCore(HttpRequest request)
        {
            return Maybe.Of(request.Form.MayGetSingle("targeturl").Bind(_ => ParseTo.Uri(_, UriKind.Relative)));
        }
    }
}

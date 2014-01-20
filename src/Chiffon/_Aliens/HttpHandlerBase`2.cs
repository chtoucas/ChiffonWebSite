namespace Narvalo.Web
{
    using System.Net;
    using System.Web;
    using Narvalo;

    public abstract class HttpHandlerBase<TQuery, TBinder> : HttpHandlerBase
        where TBinder : IQueryBinder<TQuery>, new()
    {
        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected virtual void HandleBindingFailure(HttpResponse response, string errorMessage)
        {
            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(errorMessage);
        }

        protected override void ProcessRequestCore(HttpContext context)
        {
            DebugCheck.NotNull(context);

            // Liaison du modèle.
            var binder = new TBinder();

            var query = binder.Bind(context.Request);

            if (binder.Validate()) {
                HandleBindingFailure(context.Response, "FIXME");
                return;
            }

            ProcessRequestCore(context, query);
        }
    }

}

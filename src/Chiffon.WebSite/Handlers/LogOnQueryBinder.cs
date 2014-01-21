namespace Chiffon.Handlers
{
    using System;
    using System.Web;
    using Narvalo;
    using Narvalo.Linq;
    using Narvalo.Web;

    public class LogOnQueryBinder : QueryBinderBase<LogOnQuery>
    {
        public LogOnQueryBinder() : base() { }

        protected override LogOnQuery BindCore(HttpRequest request)
        {
            var result = new LogOnQuery();

            var form = request.Form;

            form.MayGetValue("email")
                .OnSome(_ => result.Email = _);

            form.MayGetValue("password")
                .OnSome(_ => result.Password = _);

            result.TargetUrl = form.MayGetValue("targeturl")
                 .Bind(_ => MayCreate.Uri(_, UriKind.Relative));

            return result;
        }
    }
}

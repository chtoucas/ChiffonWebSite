﻿namespace Chiffon.Handlers
{
    using System;
    using System.Web;

    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Web;

    public class LogOnQueryBinder : HttpQueryBinderBase<LogOnQuery>
    {
        public LogOnQueryBinder() : base() { }

        protected override Maybe<LogOnQuery> BindCore(HttpRequest request)
        {
            var form = request.Form;

            return
                from email in form.MayGetSingle("email")
                from password in form.MayGetSingle("password")
                let targetUrl = form.MayGetSingle(Constants.SiteMap.TargetUrl).Bind(_ => ParseTo.Uri(_, UriKind.Relative))
                select new LogOnQuery {
                    Email = email,
                    Password = password,
                    TargetUrl = targetUrl
                };
        }
    }
}

﻿namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.Web;
    using Narvalo.Collections;
    using Narvalo.Web;

    [Serializable]
    public class PatternQuery
    {
        public static readonly string IdKey = "id";
    }

    public class PatternHandler : IHttpHandler
    {
        const int HoursInCache_ = 1;

        #region IHttpHandler

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var query = context.Request.QueryString;

            var reference = query.MayGetValue(PatternQuery.IdKey);
            if (reference.IsNone) { ; }

            // TODO
            string path = @"J:\home\github\ChiffonWebSite\src\Chiffon.WebSite\patterns\viviane-devaux\motif1_apercu.jpg";

            // Deliver the thumbnail.
            context.Response.Clear();
            context.Response.PubliclyCacheFor(0, HoursInCache_, 0);
            context.Response.ContentType = "image/jpeg";
            context.Response.TransmitFile(path);
        }

        #endregion
    }
}

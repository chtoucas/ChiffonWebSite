namespace Chiffon.WebSite.Handlers
{
    using System;
    using System.Web;
    using Narvalo.Web;

    public class PatternHandler : IHttpHandler
    {
        const int DaysInCache_ = 30;

        #region IHttpHandler

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // TODO
            string path = @"J:\home\github\ChiffonWebSite\src\Chiffon.WebSite\patterns\viviane-devaux\motif1_apercu.jpg";

            // Deliver the thumbnail.
            context.Response.Clear();
            context.Response.PubliclyCacheFor(DaysInCache_, 0, 0);
            context.Response.ContentType = "image/jpeg";
            context.Response.TransmitFile(path);
        }

        #endregion
    }
}

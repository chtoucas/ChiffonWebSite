namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;

    public static class ChiffonEnvironment
    {
        static object Lock_ = new Object();
        static ChiffonEnvironmentBase Current_ 
            = new DefaultChiffonEnvironment(new Uri("http://pourquelmotifsimone.com"));

        public static ChiffonEnvironmentBase Current
        {
            get { return Current_; }
            private set { lock (Lock_) { Current_ = value; } }
        }

        public static ChiffonEnvironmentBase ResolveAndInitialize(HttpRequest request)
        {
            if (request.Url.Host == "en.pourquelmotifsimone.com") {
                Current_ = new EnglishChiffonEnvironment(new Uri("http://en.pourquelmotifsimone.com"));
            }

            Current_.Initialize();

            return Current_;
        }

        public static Uri GetBaseUri(HttpRequestBase request)
        {
            return new Uri(request.Url.GetLeftPart(UriPartial.Authority), UriKind.Absolute);

            // return VirtualPathUtility.ToAbsolute("~/");
            // return new Uri(request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
            // return new Uri(request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped));
        }
    }
}
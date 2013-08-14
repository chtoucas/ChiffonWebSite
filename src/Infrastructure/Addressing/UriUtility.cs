namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using System.Globalization;
    using System.Web;

    public static class UriUtility
    {
        // Cf. http://stackoverflow.com/questions/7413466/how-can-i-get-the-baseurl-of-site
        public static Uri GetBaseUri(HttpRequestBase request)
        {
            return new Uri(request.Url.GetLeftPart(UriPartial.Authority), UriKind.Absolute);
        }

        //public static Uri GetBaseUrl(bool absolute)
        //{
        //    return absolute
        //        ? BaseUrl
        //        : new Uri(BaseUrl.GetComponents(UriComponents.Path, UriFormat.Unescaped));
        //}

        //public static Uri ToAbsolute(Uri url)
        //{
        //    if (url.IsAbsoluteUri) {
        //        return url;
        //    }

        //    Uri result;
        //    if (Uri.TryCreate(BaseUrl, url, out result)) {
        //        return result;
        //    }

        //    throw new InvalidOperationException();
        //}
    }

}
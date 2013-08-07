namespace Chiffon.Infrastructure.Addressing
{
    using System;
    using Narvalo;
    using Pacr.BuildingBlocks.Membership;

    public class DefaultSiteMapFactory : ISiteMapFactory
    {
        readonly Uri _baseUri;

//        public DefaultSiteMapFactory(WebSiteSettings settings)
//            : this(settings.BaseUrl) { }

        public DefaultSiteMapFactory(Uri baseUri)
        {
            Requires.NotNull(baseUri, "baseUri");

            _baseUri = baseUri;
        }

        public ISiteMap CreateMap(MemberId memberId)
        {
            if (memberId == MemberId.Anonymous) {
                return new DefaultSiteMap(_baseUri);
            }
            else {
                return new DefaultMemberSiteMap(memberId, _baseUri);
            }
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

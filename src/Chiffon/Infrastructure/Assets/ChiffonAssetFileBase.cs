namespace Chiffon.Infrastructure.Assets
{
    using System;
    using Narvalo;
    using Narvalo.Web.UI.Assets;

    public abstract class ChiffonAssetFileBase : AssetFile
    {
        readonly Uri _baseUrl;

        Uri _url;

        protected ChiffonAssetFileBase(Uri baseUrl, string relativePath)
            : base(relativePath)
        {
            Requires.NotNull(baseUrl, "baseUrl");

            _baseUrl = baseUrl;
        }

        public override Uri Url
        {
            get
            {
                if (_url == null) {
                    _url = new Uri(_baseUrl, VirtualPath);
                }

                return _url;
            }
        }

        protected abstract string VirtualPath { get; }
    }
}


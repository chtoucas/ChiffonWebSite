namespace Chiffon.Infrastructure.Assets
{
    using System;
    using System.Globalization;

    public class ChiffonImageFile : ChiffonAssetFileBase
    {
        public ChiffonImageFile(Uri baseUrl, string relativePath)
            : base(baseUrl, relativePath) { }

        protected override string VirtualPath
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, "img/{0}", RelativePath);
            }
        }
    }
}


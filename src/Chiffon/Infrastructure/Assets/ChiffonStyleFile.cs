namespace Chiffon.Infrastructure.Assets
{
    using System;
    using System.Globalization;

    public class ChiffonStyleFile : ChiffonAssetFileBase
    {
        public ChiffonStyleFile(Uri baseUrl, string relativePath)
            : base(baseUrl, relativePath) { }

        protected override string VirtualPath
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, "css/{0}", RelativePath);
            }
        }
    }
}


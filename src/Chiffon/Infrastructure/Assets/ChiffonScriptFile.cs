namespace Chiffon.Infrastructure.Assets
{
    using System;
    using System.Globalization;

    public class ChiffonScriptFile : ChiffonAssetFileBase
    {
        public ChiffonScriptFile(Uri baseUrl, string relativePath)
            : base(baseUrl, relativePath) { }

        protected override string VirtualPath
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, "js/{0}", RelativePath);
            }
        }
    }
}


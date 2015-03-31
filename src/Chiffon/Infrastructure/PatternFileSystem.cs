namespace Chiffon.Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;

    using Narvalo;

    public class PatternFileSystem
    {
        private readonly ChiffonConfig _config;

        public PatternFileSystem(ChiffonConfig config)
        {
            _config = config;
        }

        public string GetPath(PatternImage image)
        {
            Require.NotNull(image, "image");
            Contract.Ensures(Contract.Result<string>() != null);

            return Path.Combine(_config.PatternDirectory, image.RelativePath);
        }
    }
}

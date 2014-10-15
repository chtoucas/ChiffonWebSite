namespace Chiffon.Infrastructure
{
    using System;
    using System.IO;
    using Chiffon.Infrastructure;
    using Narvalo;

    public class PatternFileSystem
    {
        readonly ChiffonConfig _config;

        public PatternFileSystem(ChiffonConfig config)
        {
            _config = config;
        }

        public string GetPath(PatternImage image)
        {
            Require.NotNull(image, "image");

            return Path.Combine(_config.PatternDirectory, image.RelativePath);
        }
    }
}
